using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartUpTemplate.Core.ServiceRegister;

public static partial  class ServiceRegister
{
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

}