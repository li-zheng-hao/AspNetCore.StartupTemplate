using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AspNetCore.StartUpTemplate.Core.ServiceRegister;

public static partial  class ServiceRegister
{
    public static IMvcBuilder AddCustomJson(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddNewtonsoftJson(p =>
        {
            p.SerializerSettings.Formatting = Formatting.None;
            //数据格式首字母小写 不使用驼峰
            p.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //不使用驼峰样式的key
            //p.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //忽略循环引用
            p.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            p.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";
        });
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "yyyy/MM/dd HH:mm:ss",
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        return mvcBuilder;
    }
}