using System.Reflection;
using AspNetCore.CacheOutput.Redis.Extensions;
using AspNetCore.StartUpTemplate.Auth;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.Filter;
// using AspNetCore.StartupTemplate.Logging.Log;
using AspNetCore.StartUpTemplate.Mapping;
using AspNetCore.StartupTemplate.Redis;
using AspNetCore.StartupTemplate.Snowflake.SnowFlake.Redis;
using AspNetCore.StartUpTemplate.Webapi.Startup;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

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
    .AddSnowflakeWithRedis()
    .AddCustomSwaggerGen()
    .AddFreeSql()
    .AddCustomCors()
    .AddAutoMapper()
    .AddRedisManager()
    .AddRedisCacheOutput(AppSettingsConstVars.RedisConn)
    .AddDtm()
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

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>((c) =>
{
    c.RegisterModule(new AutofacModuleRegister());
});

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

var container = app.Services.GetAutofacRoot();
IocHelper.container = container;

#endregion

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

#region Spring事务管理器中间件

app.Use(async (context, next) =>
{
    TransactionalAttribute.SetServiceProvider(context.RequestServices);
    await next();
});

#endregion

app.UseCors();

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