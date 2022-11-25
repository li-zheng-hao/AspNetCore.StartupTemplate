using AspNetCore.StartupTemplate.CacheAsync.Extensions;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.Core.Serilog;
using AspNetCore.StartUpTemplate.Core.ServiceRegister;
using AspNetCore.StartUpTemplate.Core.Transaction;
using AspNetCore.StartupTemplate.DbMigration;
using AspNetCore.StartUpTemplate.Filter;
using AspNetCore.StartupTemplate.Job;
using AspNetCore.StartupTemplate.Snowflake;
using Hangfire;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Serilog
var logger = LogSetup.InitSeialog(builder.Configuration);
builder.Host.UseSerilog(logger, dispose: true);
#endregion

builder.Services.AddControllers().AddControllersAsServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddConfiguration()
    .AddSnowflakeGenerator()
    .AddCustomSwaggerGen()
    .AddFreeSql(typeof(Program).Assembly)
    .AddCustomCors()
    .AddMapster()
    .AddFreeRedis(builder.Configuration)
    .AddCustomRedisCacheOutput(builder.Configuration)
    // .AddDtm(builder.Configuration)
    .AddCustomHangfireService()
    .AddDbMigration()
    .AddCustomCAP(builder.Configuration)
    .AddHttpContextAccessor()
    .AddRedisCaching()
    .AddCustomAuthentication(builder.Configuration)
    .AddCustomMiniProfiler()
    .AddMvc(options =>
    {
        // 实体验证
        // options.Filters.Add<ModelValidatorFilter>();
        //异常处理
        options.Filters.Add<GlobalExceptionsFilter>();
    })
    .AddCustomJson();





#region IOC配置

builder.Host.UseDefaultServiceProvider(options =>
    options.ValidateScopes = true);

#endregion

var app = builder.Build();
ServiceProviderLocator.RootServiceProvider=app.Services;

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


// 开发和测试环境都开启Swagger
if (app.Environment.IsDevelopment()||app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region 中间件
app.Use(async (context, next) =>
{
    TransactionalAttribute.SetServiceProvider(context.RequestServices);
    await next();
});
app.UseMiddleware<CachingMiddleware>();
app.UseJobMiddleware();
app.UseMiniProfilerMiddleware();
// app.UseHangfireDashboard();
#endregion

// #region  HangfireJob
// HangfireJobService.Start();
// #endregion


app.UseCors();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

// 用于集成测试
public partial class Program{}