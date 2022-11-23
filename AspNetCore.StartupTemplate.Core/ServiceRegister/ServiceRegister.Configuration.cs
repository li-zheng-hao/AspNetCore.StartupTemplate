using System.Reflection;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Configuration.Option;
using Microsoft.Extensions.DependencyInjection;
using Quickwire;
using Serilog;

namespace AspNetCore.StartUpTemplate.Core.ServiceRegister;

public static partial  class ServiceRegister
{
    public static IServiceCollection AddConfiguration(this IServiceCollection serviceCollection)
    {
        AppDomain.CurrentDomain.GetAssemblies().Where(it=>it.FullName.Contains("AspNetCore.StartupTemplate")).ToList().ForEach(it =>
        {
            Log.Information(it.FullName);
            serviceCollection.ScanAssembly(it, type => true,ServiceDescriptorMergeStrategy.Replace);
        });
        return serviceCollection;
    }

}