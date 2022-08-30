using Autofac;
using System.Reflection;
using Autofac.Extras.DynamicProxy;

namespace AspNetCore.StartUpTemplate.Core;

public class AutofacModuleRegister : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var basePath = AppContext.BaseDirectory;

        #region 注入事务AOP类
        Assembly assembly = Assembly.Load("AspNetCore.StartupTemplate.Aop");
        if (assembly == null)
            throw new Exception("Aop.dll丢失了,请判断对应文件在目录下是否存在");
        
        var types = assembly.GetTypes().ToArray();
        builder.RegisterTypes(types).PropertiesAutowired();
        #endregion

        #region 注入工作单元

        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope().PropertiesAutowired();

        #endregion
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
        //支持属性注入依赖重复 每个请求一个实例 开启AOP拦截
        builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces().InstancePerLifetimeScope()
            .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).EnableInterfaceInterceptors();

        // 获取 Repository.dll 程序集服务，并注册
        var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
        //支持属性注入依赖重复 每个请求只有一个实例
        builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces().InstancePerLifetimeScope()
            .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

        #endregion

    }
}
