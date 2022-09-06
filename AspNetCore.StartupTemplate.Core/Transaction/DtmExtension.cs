using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Core;
using Autofac;
using Dapper;
using DtmCommon;
using Serilog;
using System.Data;
using System.Data.Common;

namespace AspNetCore.StartUpTemplate.Webapi;

/// <summary>
/// Dtm关于FreeSql的扩展 子事务屏障
/// </summary>
public static class DtmBarrierExtension
{
    public static async Task Call(this BranchBarrier barrier, DbTransaction tx, Func<DbTransaction, Task> busiCall)
    {
        barrier.BarrierID = barrier.BarrierID + 1;
        var bid = barrier.BarrierID.ToString().PadLeft(2, '0');
        using var scope = IocHelper.GetNewILifeTimeScope();
        var dbutils = scope.Resolve<DbUtils>();
        try
        {
            var originOp = Constant.Barrier.OpDict.TryGetValue(barrier.Op, out var ot) ? ot : string.Empty;

            var (originAffected, oEx) = await dbutils.InsertBarrier(tx, barrier.TransType, barrier.Gid,
                barrier.BranchID,
                originOp, bid, barrier.Op);
            if (oEx != null || tx.Connection.State != ConnectionState.Open)
            {
                throw new DtmOngingException(oEx?.Message);
            }

            var (currentAffected, rEx) = await dbutils.InsertBarrier(tx, barrier.TransType, barrier.Gid,
                barrier.BranchID,
                barrier.Op, bid, barrier.Op);
            if (rEx != null || tx.Connection.State != ConnectionState.Open)
            {
                throw new DtmOngingException(rEx?.Message);
            }

            Log.Information("originAffected: {originAffected} currentAffected: {currentAffected}", originAffected,
                currentAffected);

            if (string.IsNullOrWhiteSpace(rEx?.Message) && barrier.Op.Equals(Constant.TYPE_MSG) && currentAffected == 0)
                throw new DtmDuplicatedException();

            if (oEx != null || rEx != null)
            {
                throw oEx ?? rEx;
            }

            var isNullCompensation = IsNullCompensation(barrier.Op, originAffected);
            var isDuplicateOrPend = IsDuplicateOrPend(currentAffected);

            if (isNullCompensation || isDuplicateOrPend)
            {
                Log.Information(
                    "Will not exec busiCall, isNullCompensation={isNullCompensation}, isDuplicateOrPend={isDuplicateOrPend}",
                    isNullCompensation, isDuplicateOrPend);
                return;
            }

            await busiCall.Invoke(tx);
        }
        catch (DtmException e)
        {
            Log.Information($"dtm known {e.Message}, gid={barrier.Gid}, trans_type={barrier.TransType}");
            throw;
        }
    }

    /// <summary>
    /// 空补偿
    /// </summary>
    /// <param name="op"></param>
    /// <param name="originAffected"></param>
    /// <returns></returns>
    public static bool IsNullCompensation(string op, int originAffected)
        => (op.Equals(Cancel) || op.Equals(Compensate)) && originAffected > 0;

    private static readonly string Cancel = "cancel";
    private static readonly string Compensate = "compensate";

    /// <summary>
    /// 这个是重复请求或者悬挂
    /// </summary>
    /// <param name="currentAffected"></param>
    /// <returns></returns>
    public static bool IsDuplicateOrPend(int currentAffected)
        => currentAffected == 0;
}

/// <summary>
/// Dtm关于FreeSql的扩展 工具类
/// </summary>
public static class DtmDbUtilsExtension
{
    public static async Task<(int, Exception)> InsertBarrier(this DbUtils utils, DbTransaction tx, string transType,
        string gid, string branchID, string op, string barrierID, string reason)
    {
        if (string.IsNullOrWhiteSpace(op)) return (0, null);
        using var scope = IocHelper.GetNewILifeTimeScope();
        var _specialDelegate = scope.Resolve<DbSpecialDelegate>();
        try
        {
            var str = string.Concat(AppSettingsConstVars.DtmBarrierTableName,
                "(trans_type, gid, branch_id, op, barrier_id, reason) values(@trans_type,@gid,@branch_id,@op,@barrier_id,@reason)");
            var sql = _specialDelegate.GetDbSpecial().GetInsertIgnoreTemplate(str, Constant.Barrier.PG_CONSTRAINT);

            sql = _specialDelegate.GetDbSpecial().GetPlaceHoldSQL(sql);
            var affected = await tx.Connection.ExecuteAsync(
                sql,
                new
                {
                    trans_type = transType,
                    gid = gid,
                    branch_id = branchID,
                    op = op,
                    barrier_id = barrierID,
                    reason = reason
                },
                transaction: tx);

            return (affected, null);
        }
        catch (Exception ex)
        {
            return (0, ex);
        }
    }
}