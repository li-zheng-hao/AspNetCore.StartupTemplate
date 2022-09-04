using System.Reflection;
using AspNetCore.StartUpTemplate.Core;
using Autofac;
using Autofac.Extras.DynamicProxy;

namespace AspNetCore.StartUpTemplate.Webapi.Startup;

public class AutofacModuleRegister : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        
        Assembly[] assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "AspNetCore.StartupTemplate.*.dll").Select(Assembly.LoadFrom).ToArray();
        
        #region 带有接口层的服务注入
        var assemblyServicesDll = assemblies.FirstOrDefault(it => it.FullName.Contains(".Services"));
        var assemblyRepositoryDll = assemblies.FirstOrDefault(it => it.FullName.Contains(".Repository"));
        if (assemblyServicesDll==null || assemblyRepositoryDll==null)
        {
            var msg = "Repository.dll和Services.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
            throw new Exception(msg);
        }
        
        // 获取 Service.dll 程序集服务，并注册
        //支持属性注入依赖重复 每个请求一个实例 开启AOP拦截
        builder.RegisterAssemblyTypes(assemblyServicesDll).AsImplementedInterfaces().InstancePerLifetimeScope()
            .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).EnableInterfaceInterceptors();

        // 获取 Repository.dll 程序集服务，并注册
        //支持属性注入依赖重复 每个请求只有一个实例
        // builder.RegisterAssemblyTypes(assemblyRepositoryDll).AsImplementedInterfaces().InstancePerLifetimeScope()
        //     .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        
        #endregion
        builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
    }
}
