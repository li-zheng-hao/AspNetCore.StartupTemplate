using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace AspNetCore.StartUpTemplate.Configuration.Option;

[RegisterService(ServiceLifetime.Singleton)]
public class MysqlOption
{
    [InjectConfiguration("Mysql:ConnectionString")]
    public string ConnectionString { get; set; }
}