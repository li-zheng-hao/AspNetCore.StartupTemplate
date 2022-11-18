using AspNetCore.StartUpTemplate.Configuration.Option;
using Dtmcli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartUpTemplate.Core.ServiceRegister;

public static partial  class ServiceRegister
{
    #region DTM

    /// <summary>
    /// 配置DTM
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDtm(this IServiceCollection services,IConfiguration configuration)
    {
        var dtmOption = configuration.GetSection("Dtm").Get<DtmOption>();
        services.AddDtmcli(dtm =>
        {
            dtm.DtmUrl = dtmOption.DtmUrl;
            dtm.BarrierTableName =dtmOption.DtmBarrierTableName;
        });
        return services;
    }

    #endregion

}