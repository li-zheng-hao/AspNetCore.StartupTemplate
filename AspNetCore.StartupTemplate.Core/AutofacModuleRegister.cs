using Autofac;
using System.Reflection;
namespace AspNetCore.StartUpTemplate.Core;

public class AutofacModuleRegister : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var basePath = AppContext.BaseDirectory;
      
        #region 带有接口层的服务注入

        var servicesDllFile = Path.Combine(basePath, "AspNetCore.StartupTemplate.Services.dll");
        var repositoryDllFile = Path.Combine(basePath, "AspNetCore.StartupTemplate.Repository.dll");

        if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
        {
            var msg = "Repository.dll和Services.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
            throw new Exception(msg);
        }

        // 获取 Service.dll 程序集服务，并注册
        var assemblysServices = Assembly.LoadFrom(servicesDllFile);
        //支持属性注入依赖重复 每个请求一个实例
        builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces().InstancePerLifetimeScope()
            .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

        // 获取 Repository.dll 程序集服务，并注册
        var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
        //支持属性注入依赖重复 每个请求只有一个实例
        builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces().InstancePerLifetimeScope()
            .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

        #endregion

    }
}
