using AspNetCore.StartUpTemplate.Configuration;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace AspNetCore.StartUpTemplate.Core;

public class UnitOfWork:IUnitOfWork
{
    private ISqlSugarClient _sqlSugarClient;
    private readonly ILogger _logger;
    private bool _isUsingTransaction = false;// 是否使用了事务
    private bool _isNestedTrans;
    private string _savepointName;

    public UnitOfWork(SqlSugarScope dbScoped,ILogger<UnitOfWork> logger)
    {
        _sqlSugarClient = dbScoped;
        _logger = logger;
    }

    /// <summary>
    ///     获取DB
    /// </summary>
    /// <returns></returns>
    public ISqlSugarClient GetDbClient()
    {
        // 必须要as，后边会用到切换数据库操作
        return _sqlSugarClient;
    }

    public void SetDbClient(ISqlSugarClient sqlSugarClient)
    {
        _sqlSugarClient = sqlSugarClient;
    }

    public void BeginTran()
    {
        GetDbClient().AsTenant().BeginTran();
        _isUsingTransaction = true;
    }

    public void CommitTran()
    {
        try
        {
            GetDbClient().AsTenant().CommitTran(); //
        }
        catch (Exception ex)
        {
            RollbackTran();
            _logger.LogError("事务提交异常",ex);
            throw;
        }
    }

    public void RollbackTran()
    {
        GetDbClient().AsTenant().RollbackTran();
    }

    public bool IsUsingTransaction()
    {
        return _isUsingTransaction;
    }

    public void Reset()
    {
        _isUsingTransaction = false;
        _sqlSugarClient = null;
    }

    public bool IsNestedTrans()
    {
        return _isNestedTrans;
    }

    public void RollbackToSavepoint(string savepoint)
    {
        GetDbClient().Ado.ExecuteCommand($"rollback to {savepoint}");
    }

    public void SetSavepoint(string savepointName)
    {
        _savepointName = savepointName;
        _isNestedTrans = true;
    }
}