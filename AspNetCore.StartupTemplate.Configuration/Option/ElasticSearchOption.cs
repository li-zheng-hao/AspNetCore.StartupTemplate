using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace AspNetCore.StartUpTemplate.Configuration.Option;
[RegisterService(ServiceLifetime.Singleton)]
public class ElasticSearchOption
{
    [InjectConfiguration("ElasticSearch:Url")]
    public string Url { get; set; }
}