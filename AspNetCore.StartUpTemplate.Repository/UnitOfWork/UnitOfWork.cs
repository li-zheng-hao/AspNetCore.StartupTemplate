using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.IRepository;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace AspNetCore.StartUpTemplate.Repository;

public class UnitOfWork:IUnitOfWork,IDisposable
{
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly ILogger _logger;
    private bool _isUsingTransaction = false;// 是否使用了事务
    public UnitOfWork(SqlSugarScope dbScoped,ILogger<UnitOfWork> logger)
    {
        _sqlSugarClient = dbScoped;
        _logger = logger;
    }

    /// <summary>
    ///     获取DB，保证唯一性
    /// </summary>
    /// <returns></returns>
    public SqlSugarScope GetDbClient()
    {
        // 必须要as，后边会用到切换数据库操作
        return _sqlSugarClient as SqlSugarScope;
    }

    public void BeginTran()
    {
        GetDbClient().BeginTran();
        _isUsingTransaction = true;
    }

    public void CommitTran()
    {
        try
        {
            GetDbClient().CommitTran(); //
        }
        catch (Exception ex)
        {
            GetDbClient().RollbackTran();
            // _logger.LogError("事务提交异常",ex);
            throw;
        }
    }

    public void RollbackTran()
    {
        GetDbClient().RollbackTran();
    }

    public void Dispose()
    {
        if (_isUsingTransaction)
        {
            CommitTran();
        }
    }
}