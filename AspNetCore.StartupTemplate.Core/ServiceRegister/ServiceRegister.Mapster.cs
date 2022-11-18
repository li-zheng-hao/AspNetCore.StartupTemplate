using System.Reflection;
using AspNetCore.StartUpTemplate.Contract.DTOs;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartUpTemplate.Core.ServiceRegister;

public static partial  class ServiceRegister
{
    public static IServiceCollection AddMapster(this IServiceCollection serviceCollection)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        // 全局忽略大小写
        TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);
        Assembly applicationAssembly = typeof(BaseDto<,>).Assembly;
        typeAdapterConfig.Scan(applicationAssembly);
        return serviceCollection;
    }
}