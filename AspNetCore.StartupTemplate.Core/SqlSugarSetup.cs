using System;
using AspNetCore.StartUpTemplate.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace AspNetCore.StartUpTemplate.Core;
/// <summary>
/// SqlSugar 启动服务
/// </summary>
public static class SqlSugarSetup
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
                };
            });
        services.AddSingleton(scope);
    }

}
