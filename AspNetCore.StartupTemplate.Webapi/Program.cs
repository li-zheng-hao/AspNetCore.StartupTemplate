using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.Core.Transaction;
using AspNetCore.StartupTemplate.DbMigration;
using AspNetCore.StartUpTemplate.Filter;
using AspNetCore.StartUpTemplate.Webapi.Startup;
using FreeScheduler.Dashboard;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Serilog配置===========================
var logger = LogSetup.InitSeialog(builder.Configuration);
builder.Host.UseSerilog(logger, dispose: true);
#endregion

builder.Services.AddControllers().AddControllersAsServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddConfigurationConfig(builder.Configuration)
    // .AddSnowflakeGenerator()
    .AddCustomSwaggerGen()
    .AddFreeSql()
    .AddCustomCors()
    .AddMapster()
    // .AddFreeRedis()
    // .AddCustomRedisCacheOutput()
    // .AddDtm()
    // .AddDbMigration()
    // .AddCustomCAP()
    .AddHttpContextAccessor()
    .AddHttpContextUser()
    // .AddScheduler()
    .AddMvc(options =>
    {
        // //实体验证
        options.Filters.Add<ModelValidatorFilter>();
        //异常处理
        options.Filters.Add<GlobalExceptionsFilter>();
    })
    .AddCustomJson();




#region IOC配置============================

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

#region 特性事务管理器中间件

app.Use(async (context, next) =>
{
    TransactionalAttribute.SetServiceProvider(context.RequestServices);
    await next();
});

#endregion


app.UseCors();

app.UseFreeSchedulerDashboard();

app.UseRouting();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();