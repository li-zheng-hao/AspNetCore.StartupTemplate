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

var logger=LogSetup.InitSeialog(builder.Configuration);
builder.Host.UseSerilog(logger, dispose: true);
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region 配置雪花ID========================

builder.Services.AddSnowflakeWithRedis(opt =>
{
    opt.InstanceName = "snowflake:";
    opt.ConnectionString = AppSettingsConstVars.RedisConn;
    opt.WorkIdLength = 9; // 9位支持512个工作节点
    opt.RefreshAliveInterval = TimeSpan.FromHours(1);
    // opt.StartTimeStamp = new DateTime(2000, 0, 0);
});

#endregion

#region 添加自定义过滤器======================

builder.Services.AddMvc(options =>
{
    //实体验证
    options.Filters.Add<ModelValidator>(); 
    //异常处理
    options.Filters.Add<GlobalExceptionsFilter>();

});

#endregion

#region 序列化=============================
builder.Services.AddControllers().AddNewtonsoftJson(p =>
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
#endregion

#region Swagger自定义配置=======================

builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
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
    c.IncludeXmlComments(xmlPath);
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
});
#endregion

#region IOC配置============================

builder.Services.AddSingleton(new AppSettingsHelper(builder.Configuration));
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>((c) =>
{
    var controllersTypesInAssembly = typeof(Program).Assembly
        .GetExportedTypes()
        .Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToArray();

    c.RegisterTypes(controllersTypesInAssembly).PropertiesAutowired();
    c.RegisterModule(new AutofacModuleRegister());
    
    
});
builder.Services.AddFreeSql();
// builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

#endregion

#region 配置跨域=========================
// 此处根据自己的需要配置可通过的域名或ip
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});
#endregion

#region AutoMapper配置=====================

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

#endregion

#region Redis相关配置====================

// redis工具类
builder.Services.AddRedisManager();

// webapi接口使用redis缓存
builder.Services.AddRedisCacheOutput(AppSettingsConstVars.RedisConn);

#endregion

#region Kestrel服务器配置===================

builder.WebHost.ConfigureKestrel((context, options) =>
{
    //设置应用服务器Kestrel请求体最大为50MB，默认为28.6MB
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 50;
});
#endregion

#region 健康检查=========================

builder.Services.AddHealthChecks();//健康检查

#endregion

var app = builder.Build();

#region IOC工具类===============================
var container= app.Services.GetAutofacRoot();
IocHelper.container = container;
#endregion
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

#region 事务管理器
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