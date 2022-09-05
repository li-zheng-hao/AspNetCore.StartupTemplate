using System.Diagnostics;
using System.Reflection;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Core.Cache;
using AspNetCore.StartupTemplate.DbMigration;
using AspNetCore.StartUpTemplate.Filter;
using AspNetCore.StartUpTemplate.Mapping;
using Dtmcli;
using FreeSql;
using FreeSql.Internal;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Core;

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
                            Trace.WriteLine($"freesql监视命令 {cmd.CommandText}");
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
                        Log.Logger.Warning($"慢sql 耗时{e.ElapsedMilliseconds}毫秒 语句{e.Sql}");
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

        #region DTM
        /// <summary>
        /// 配置DTM
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDtm(this IServiceCollection services)
        {
            services.AddDtmcli(dtm =>
            {
                dtm.DtmUrl = AppSettingsConstVars.DtmUrl;
                dtm.BarrierTableName = "barrier";
            });
            return services;
        }

        #endregion


        public static IMvcBuilder AddCustomJson(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddNewtonsoftJson(p =>
            {
                //数据格式首字母小写 不使用驼峰
                p.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //不使用驼峰样式的key
                //p.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //忽略循环引用
                p.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                p.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";
            });
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "yyyy/MM/dd HH:mm:ss",
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return mvcBuilder;
        }

        public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(c =>
            {
                //添加Authorization
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });
                // 接口文档抓取
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //... and tell Swagger to use those XML comments.
                c.IncludeXmlComments(xmlPath,true);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
                //允许上传文件
                c.OperationFilter<FileUploadFilter>();
                c.DocumentFilter<SwaggerIgnoreFilter>();
            });
            return serviceCollection;
        }

        public static IServiceCollection AddConfigurationConfig(this IServiceCollection serviceCollection,IConfiguration config)
        {
            serviceCollection.AddSingleton(new AppSettingsHelper(config));
            return serviceCollection;
        }
        public static IServiceCollection AddCustomCors(this IServiceCollection serviceCollection)
        {
            // 此处根据自己的需要配置可通过的域名或ip
            serviceCollection.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.AllowAnyOrigin();
                        policy.AllowAnyHeader();
                        policy.AllowAnyMethod();
                    });
            });
            return serviceCollection;
        }
        
        public static IServiceCollection AddAutoMapper(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(AutoMapperConfig));
            serviceCollection.AddTransient<NeedCacheAttribute>();

            return serviceCollection;
        }

    }

 

   
}
