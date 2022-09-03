using System.Data;
using System.Data.Common;
using AspNetCore.StartUpTemplate.Model;
using AspNetCore.StartUpTemplate.Utility;
using EvolveDb;
using FreeSql.Internal.ObjectPool;
using Microsoft.Extensions.Logging;

namespace AspNetCore.StartupTemplate.DbMigration;

public class DbMigation
{
    private readonly IFreeSql _fsql;
    private readonly ILogger<DbMigation> _logger;

    public DbMigation(ILogger<DbMigation> logger,IFreeSql fsql)
    {
        _logger = logger;
        _fsql = fsql;
    }
    /// <summary>
    /// 执行迁移 失败全部回滚
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void Migrate()
    {
        using Object<DbConnection> conn = _fsql.Ado.MasterPool.Get();
        var user=_fsql.Select<Users>().ToOne();
        if (conn.Value.State != ConnectionState.Open)
            throw new Exception("数据库连接未打开");
        _logger.LogWarning(Path.Combine( PathHelper.GetExecuteDir(),"db/migrations"));
        try
        {
            var evolve = new Evolve(conn.Value, msg => _logger.LogInformation(msg))
            {
                Locations = new[] { Path.Combine( PathHelper.GetExecuteDir(),"db/migrations") },
                IsEraseDisabled = true,
            };
            evolve.Migrate();
        }
        catch (Exception ex)
        {
            _logger.LogCritical("数据库迁移失败 ", ex);
            throw;
        }
    }
}