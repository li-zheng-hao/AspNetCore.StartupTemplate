using System.Diagnostics;
using System.Reflection;
using AspNetCore.StartUpTemplate.Configuration;
using FreeSql;
using FreeSql.Internal;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace AspNetCore.StartUpTemplate.Webapi.Startup
{
    public static class ServiceCollectionExtensions
    {
        #region FreeSql
        /// <summary>
        /// FreeSql
        /// </summary>
        /// <param name="services"></param>
        /// <param name="c"></param>
        public static IServiceCollection AddFreeSql(this IServiceCollection services)
        {
            Func<IServiceProvider, IFreeSql> fsql = r =>
            {
                IFreeSql fsql = new FreeSqlBuilder()
                    .UseConnectionString(DataType.MySql,AppSettingsConstVars.DbConnection)
                    .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithLower)
#if DEBUG
                    .UseAutoSyncStructure(true)
#else
                    .UseAutoSyncStructure(false)
#endif
                    
                    .UseNoneCommandParameter(true)
                    .UseMonitorCommand(cmd =>
                        {
                            Trace.WriteLine(cmd.CommandText + ";");
                        }
                    )
                    .Build()
                    .SetDbContextOptions(opt => opt.EnableCascadeSave = false);//联级保存功能开启
                fsql.Aop.CurdAfter += (s, e) =>
                {
                    Log.Debug($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}: FullName:{e.EntityType.FullName}" +
                              $" ElapsedMilliseconds:{e.ElapsedMilliseconds}ms, {e.Sql}");

                    if (e.ElapsedMilliseconds > 200)
                    {
                        //记录日志
                        //发送短信给负责人
                    }
                };



                fsql.UseJsonMap();
                return fsql;
            };

            services.AddSingleton(fsql);
            services.AddScoped<UnitOfWorkManager>();
            services.AddFreeRepository(null, typeof(Program).Assembly);

            return services;
        }
        #endregion


    }
}
