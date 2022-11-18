using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace AspNetCore.StartUpTemplate.Configuration.Option;
[RegisterService(ServiceLifetime.Singleton)]
public class DtmOption
{
    [InjectConfiguration("Dtm:DtmUrl")]
    public string DtmUrl { get; set; }
    
    [InjectConfiguration("Dtm:DtmBarrierTableName")]
    public string DtmBarrierTableName { get; set; } = "barrier";
}