using AspNetCore.CacheOutput.Redis.Extensions;
using AspNetCore.StartUpTemplate.Auth;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.Filter;
using AspNetCore.StartUpTemplate.Mapping;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Nlog配置===========================
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
builder.Logging.ClearProviders();
builder.Host.UseNLog();
#endregion


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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
builder.Services.AddSqlSugarSetup();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>((c) =>
{
    var controllersTypesInAssembly = typeof(Program).Assembly
        .GetExportedTypes()
        .Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToArray();

    c.RegisterTypes(controllersTypesInAssembly).PropertiesAutowired();
    c.RegisterModule(new AutofacModuleRegister());

});
builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
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

#region AutoMapper配置===========

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

#endregion

builder.Services.AddRedisCacheOutput(AppSettingsConstVars.RedisConn);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();