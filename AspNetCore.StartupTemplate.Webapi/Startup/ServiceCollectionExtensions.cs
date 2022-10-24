﻿using System.Diagnostics;
using System.Reflection;
using AspNetCore.CacheOutput;
using AspNetCore.CacheOutput.Redis;
using AspNetCore.StartUpTemplate.Auth.HttpContextUser;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Contract.DTOs;
using AspNetCore.StartUpTemplate.Filter;
using DotNetCore.CAP.Messages;
using Dtmcli;
using FreeRedis;
using FreeSql;
using FreeSql.Internal;
using Mapster;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using StackExchange.Redis;

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
                    .UseConnectionString(DataType.MySql, GlobalConfig.Instance.Mysql.ConnectionString)
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
                
                dtm.DtmUrl = GlobalConfig.Instance.Dtm.DtmUrl;
                dtm.BarrierTableName = "barrier";
            });
            return services;
        }

        #endregion

        #region Json配置

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

        #endregion

        #region Swagger配置

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
                c.IncludeXmlComments(xmlPath, true);
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

        #endregion

        #region 自定义配置

        public static IServiceCollection AddConfigurationConfig(this IServiceCollection serviceCollection,
            IConfiguration config)
        {
            var globalConfig=config.Get<GlobalConfig>();
            GlobalConfig.Instance = globalConfig;
            serviceCollection.AddSingleton(globalConfig);

            return serviceCollection;
        }

        #endregion

        #region 跨域

        public static IServiceCollection AddCustomCors(this IServiceCollection serviceCollection)
        {
            // 此处根据自己的需要配置可通过的域名或ip
            serviceCollection.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.SetIsOriginAllowed(it=>true);
                        policy.AllowAnyHeader();
                        policy.AllowAnyMethod();
                        policy.AllowCredentials();
                    });
            });
            return serviceCollection;
        }

        #endregion

        #region Mapster映射

        public static IServiceCollection AddMapster(this IServiceCollection serviceCollection)
        {
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            // 全局忽略大小写
            TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);
            Assembly applicationAssembly = typeof(BaseDto<,>).Assembly;
            typeAdapterConfig.Scan(applicationAssembly);
            return serviceCollection;
        }

        #endregion

        #region FreeRedis配置

        public static IServiceCollection AddFreeRedis(this IServiceCollection serviceCollection)
        {
            RedisClient redisClient = new RedisClient(
                GlobalConfig.Instance.Redis.RedisConn,
                GlobalConfig.Instance.Redis.SentinelAdders.ToArray(),
                true //是否读写分离
            );
            serviceCollection.AddSingleton<IRedisClient>(redisClient);
            return serviceCollection;
        }

        public static IServiceCollection AddCustomRedisCacheOutput(
            this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            services.TryAdd(ServiceDescriptor.Singleton<CacheKeyGeneratorFactory, CacheKeyGeneratorFactory>());
            services.TryAdd(ServiceDescriptor.Singleton<ICacheKeyGenerator, DefaultCacheKeyGenerator>());
            services.TryAdd(ServiceDescriptor.Singleton<IApiCacheOutput, StackExchangeRedisCacheOutputProvider>());
            ConfigurationOptions options = new ConfigurationOptions();
            options.Password = GlobalConfig.Instance.Redis.Password;
            options.CommandMap = CommandMap.Sentinel;
            foreach (var addr in GlobalConfig.Instance.Redis.SentinelAdders)
            {
                var ipPort = addr.Split(':');
                options.EndPoints.Add(ipPort[0], Convert.ToInt32(ipPort[1]));
            }
            options.TieBreaker = ""; //这行在sentinel模式必须加上
            options.DefaultVersion = new Version(3, 0);
            options.AllowAdmin = true;
            var masterConfig = new ConfigurationOptions
            {
                CommandMap = CommandMap.Default,
                ServiceName = GlobalConfig.Instance.Redis.ServiceName,
                Password = GlobalConfig.Instance.Redis.Password,
                AllowAdmin = true
            };
            services.TryAdd(
                ServiceDescriptor.Singleton<IConnectionMultiplexer>(
                    (IConnectionMultiplexer)ConnectionMultiplexer.Connect(options)));
            services.TryAdd(ServiceDescriptor.Transient<IDatabase>((Func<IServiceProvider, IDatabase>)(e =>
                ((ConnectionMultiplexer)e.GetRequiredService<IConnectionMultiplexer>())
                .GetSentinelMasterConnection(masterConfig).GetDatabase())));
            return services;
        }

        #endregion


        #region CAP配置

        public static IServiceCollection AddCustomCAP(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddCap(x =>
            {
                x.UseDashboard();
                x.UseMySql(GlobalConfig.Instance.Mysql.ConnectionString);
                x.UseRabbitMQ(it =>
                {
                    it.HostName = GlobalConfig.Instance.RabbitMQ.HostName;
                    it.Port =GlobalConfig.Instance.RabbitMQ.Port ;
                    it.UserName = GlobalConfig.Instance.RabbitMQ.UserName;
                    it.Password = GlobalConfig.Instance.RabbitMQ.Password;
                    it.VirtualHost = GlobalConfig.Instance.RabbitMQ.VirtualHost;
                });
                x.FailedRetryCount = 5;
                x.FailedThresholdCallback = failed =>
                {
                    var logger = failed.ServiceProvider.GetService<ILogger<Program>>();
                    logger.LogError($@"A message of type {failed.MessageType} failed after executing {x.FailedRetryCount} several times, 
                        requiring manual troubleshooting. Message name: {failed.Message.GetName()}");
                    // todo 短信/邮件通知异常
                };
            });
            return serviceCollection;
        }

        #endregion

        #region NewtonSoftJson配置自定义
        /// <summary>
        /// TODO 自定义json配置
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomJson(this IServiceCollection serviceCollection)
        {
            
            return serviceCollection;
        }

        #endregion
        
        #region HttpContextUser注入
        /// <summary>
        /// TODO 自定义json配置
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpContextUser(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IHttpContextUser, HttpContextUser>();
            return serviceCollection;
        }

        #endregion
        
    }
}