using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Configuration.Option;
using AspNetCore.StartUpTemplate.Core.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quickwire;

namespace AspNetCore.StartUpTemplate.Core.ServiceRegister;

public static partial  class ServiceRegister
{
    #region 权限校验部分
    /// <summary>
    /// 添加默认的Jwt认证
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection serviceCollection,IConfiguration configuration)
    {
        var jwtOption = configuration.GetSection("Jwt").Get<JwtOption>();
        
        AuthenticationBuilder builder = new AuthenticationBuilder(serviceCollection);
        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
            JwtBearerDefaults.AuthenticationScheme,
            opt =>
            {
                opt.TokenValidationParameters = JwtHelper.GetTokenValidatonParameter(jwtOption);
            });
        return serviceCollection;
    }
    #endregion
}