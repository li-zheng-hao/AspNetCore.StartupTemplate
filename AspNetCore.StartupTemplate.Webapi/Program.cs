using System.Reflection;
using AspNetCore.CacheOutput.Redis.Extensions;
using AspNetCore.StartUpTemplate.Auth;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartupTemplate.CustomScheduler;
using AspNetCore.StartupTemplate.DbMigration;
using AspNetCore.StartUpTemplate.Filter;
using AspNetCore.StartupTemplate.Snowflake;
// using AspNetCore.StartupTemplate.Logging.Log;
using AspNetCore.StartUpTemplate.Webapi.Startup;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FreeRedis;
using FreeScheduler.Dashboard;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

#region Serilog配置===========================

var logger = LogSetup.InitSeialog(builder.Configuration);
builder.Host.UseSerilog(logger, dispose: true);

#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services
    .AddConfigurationConfig(builder.Configuration)
    .AddSnowflakeGenerator()
    .AddCustomSwaggerGen()
    .AddFreeSql()
    .AddCustomCors()
    .AddMapster()
    .AddFreeRedis()
    .AddCustomRedisCacheOutput()
    .AddDtm()
    .AddDbMigration()
    .AddCustomCAP()
    .AddScheduler()
    .AddMvc(options =>
    {
        // //实体验证
        options.Filters.Add<ModelValidator>();
        //异常处理
        options.Filters.Add<GlobalExceptionsFilter>();
    })
    .AddCustomJson();


builder.Services.AddControllers();


#region IOC配置============================

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>((c) =>
    { 
        c.RegisterModule(new AutofacModuleRegister());
    });

// builder.Host.UseDefaultServiceProvider(options =>
    // options.ValidateScopes = true);

#endregion

#region Kestrel服务器配置===================

builder.WebHost.ConfigureKestrel((context, options) =>
{
    //设置应用服务器Kestrel请求体最大为50MB，默认为28.6MB
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 50;
});

#endregion

#region 健康检查=========================

builder.Services.AddHealthChecks(); //健康检查

#endregion



var app = builder.Build();
#region IOC工具类===============================
// 全局的生命周期
var globalLifetimeScope = app.Services.GetAutofacRoot();
IocHelper.SetGlobalLifeTimeScope( globalLifetimeScope);

#endregion

#region 启动项目时执行数据库迁移

// 生产环境需要执行，先用freesql生成差异化迁移脚本后放在db/migrations目录下在发布到生产环境执行
// 开发环境测试环境(同一个数据库)通过freesql自动同步
if(app.Environment.IsDevelopment()==false){
    using (var scope = app.Services.CreateScope())
    {
        scope.ServiceProvider.MigrateDatabase();
    }
}



#endregion


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Spring事务管理器中间件

app.Use(async (context, next) =>
{
    TransactionalAttribute.SetServiceProvider(context.RequestServices);
    await next();
});

#endregion

#region 程序正常退出取消注册的雪花ID

app.Lifetime.ApplicationStopping.Register(it =>
{
    var sp = it as IServiceProvider;
    sp.GetService<SnowflakeWorkIdManager>()?.UnRegisterWorkId();
},app.Services);
#endregion

app.Use(async (req,req2) =>
{
    Console.WriteLine("a");
    await req2();
});

app.UseCors();
app.UseFreeSchedulerDashboard(it =>
{
    // it.DashboardAuthorizationFilter = null;// 自定义权限校验
    // it.TaskTableName // 表名
    // it.TaskLogTableName // 日志表名
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting().UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = s => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});
// app.MapControllers();

app.Run();