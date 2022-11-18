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
        // serviceCollection.ScanAssembly(typeof(JwtOption).Assembly,it=>true);
        // var basePath = AppContext.BaseDirectory;
        // var servicesDllFile = Path.Combine(basePath, "AspNetCore.StartupTemplate.Core.dll");
        // var repositoryDllFile = Path.Combine(basePath, "AspNetCore.StartupTemplate.Configuration.dll");
        // var webapiDll = Path.Combine(basePath, "AspNetCore.StartupTemplate.Webapi.dll");
        // var assembly = Assembly.LoadFrom(servicesDllFile);
        // var assembly2 = Assembly.LoadFrom(repositoryDllFile);
        // var webapiDllAssembly = Assembly.LoadFrom(webapiDll);
        // serviceCollection.ScanAssembly(assembly, it => true);
        // serviceCollection.ScanAssembly(assembly2, it => true);
        // serviceCollection.ScanAssembly(webapiDllAssembly, it => true);
        AppDomain.CurrentDomain.GetAssemblies().Where(it=>it.FullName.Contains("AspNetCore.StartupTemplate")).ToList().ForEach(it =>
        {
            Log.Information(it.FullName);
            serviceCollection.ScanAssembly(it, type => true,ServiceDescriptorMergeStrategy.Replace);
        });
        return serviceCollection;
    }

}