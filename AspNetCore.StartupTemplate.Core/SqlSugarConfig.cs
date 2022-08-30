using System;
using AspNetCore.StartUpTemplate.Configuration;
using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace AspNetCore.StartUpTemplate.Core;
/// <summary>
/// SqlSugar 启动服务
/// </summary>
public static class SqlSugarConfig
{
    public static void AddSqlSugarSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        var scope=new SqlSugarScope(new ConnectionConfig()
            {
                ConnectionString = AppSettingsConstVars.DbConnection,//连接符字串
                DbType = DbType.MySql,//数据库类型
                IsAutoCloseConnection = true, //不设成true要手动close
                MoreSettings = new ConnMoreSettings(){
                    DefaultCacheDurationInSeconds = 60,
                    IsAutoRemoveDataCache = true
                }
            },
            db=> {
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    NLog.LogManager.GetCurrentClassLogger().Info("lzh"+sql);
                };
            });
        services.AddSingleton(scope);
        
    }
    public static SqlSugarClient GetSugarClient()
    {
        var client = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = AppSettingsConstVars.DbConnection, //连接符字串
            DbType = DbType.MySql, //数据库类型
            IsAutoCloseConnection = true, //不设成true要手动close
            MoreSettings = new ConnMoreSettings()
            {
                DefaultCacheDurationInSeconds = 60,
                IsAutoRemoveDataCache = true
            }
        });
        client.Aop.OnLogExecuting = (sql, pars) =>
        {
            NLog.LogManager.GetCurrentClassLogger().Info("lzh"+sql);
        };
        return client;
    }
}
