using System.Reflection;
using AspNetCore.StartUpTemplate.Filter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace AspNetCore.StartUpTemplate.Core.ServiceRegister;

public static partial  class ServiceRegister
{
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
            var webapi = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
            var webapiPath = Path.Combine(AppContext.BaseDirectory, webapi);
            if(File.Exists(webapiPath))
                c.IncludeXmlComments(webapiPath, true);
            var contract = $"{Assembly.GetEntryAssembly().GetReferencedAssemblies().Where(it=>it.Name.Contains("Contract")).FirstOrDefault()?.Name}.xml";
            var contractPath = Path.Combine(AppContext.BaseDirectory, contract);
            if(File.Exists(contractPath))
                c.IncludeXmlComments(contractPath, false);
            
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
}