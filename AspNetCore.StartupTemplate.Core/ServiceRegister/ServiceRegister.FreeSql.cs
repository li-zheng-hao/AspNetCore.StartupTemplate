using System.Diagnostics;
using System.Reflection;
using AspNetCore.StartUpTemplate.Configuration.Option;
using FreeSql;
using FreeSql.Internal;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AspNetCore.StartUpTemplate.Core.ServiceRegister;

public static partial  class ServiceRegister
{
    #region FreeSql

    /// <summary>
    /// FreeSql
    /// </summary>
    /// <param name="services"></param>
    /// <param name="c"></param>
    public static IServiceCollection AddFreeSql(this IServiceCollection services,Assembly assemblies)
    {
        Func<IServiceProvider, IFreeSql> fsql = sp =>
        {
            var mysqlOption = sp.GetService<MysqlOption>();
            IFreeSql fsql = new FreeSqlBuilder()
                .UseConnectionString(DataType.MySql, mysqlOption.ConnectionString)
                .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithLower)
#if DEBUG
                .UseAutoSyncStructure(true)
#else
                    .UseAutoSyncStructure(false)
#endif

                .UseNoneCommandParameter(true)
                .UseMonitorCommand(cmd => { Trace.WriteLine($"freesql监视命令 {cmd.CommandText}"); }
                )
                .Build()
                .SetDbContextOptions(opt => opt.EnableCascadeSave = false); //联级保存功能开启
            fsql.Aop.CurdAfter += (s, e) =>
            {
                if (e.ElapsedMilliseconds > 200)
                {
                    //记录日志
                    //发送短信给负责人
                    Log.Logger.Warning($"慢sql 耗时{e.ElapsedMilliseconds}毫秒 语句{e.Sql}");
                }
            };

            fsql.UseJsonMap();
            return fsql;
        };

        services.AddSingleton(fsql);
        services.AddScoped<UnitOfWorkManager>();
        services.AddFreeRepository(null, assemblies);

        return services;
    }

    #endregion
}