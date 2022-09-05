using System.Reflection;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.Core.Cache;
using AspNetCore.StartupTemplate.Redis;
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
        if (assemblyServicesDll==null)
        {
            var msg = "Services.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
            throw new Exception(msg);
        }
        
        // 获取 Service.dll 程序集服务，并注册
        //支持属性注入依赖重复 每个请求一个实例 开启AOP拦截
        builder.RegisterAssemblyTypes(assemblyServicesDll).AsImplementedInterfaces().InstancePerLifetimeScope()
            .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).EnableInterfaceInterceptors();

        #endregion
        builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
    }
}
