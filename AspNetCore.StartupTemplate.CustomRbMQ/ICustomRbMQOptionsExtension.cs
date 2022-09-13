using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartupTemplate.CustomRbMQ;

public interface ICustomRbMQOptionsExtension
{
    
    /// <summary>
    /// Registered child service.
    /// </summary>
    /// <param name="services">add service to the <see cref="IServiceCollection" /></param>
    void AddServices(IServiceCollection services);
}