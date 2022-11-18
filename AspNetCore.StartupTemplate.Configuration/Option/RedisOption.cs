using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace AspNetCore.StartUpTemplate.Configuration.Option;

[RegisterService(ServiceLifetime.Singleton)]
public class RedisOption
{
    [InjectConfiguration("RedisOption:ServiceName")]
    public string ServiceName { get; set; }

    [InjectConfiguration("RedisOption:Password")]
    public string Password { get; set; }

    [InjectConfiguration("RedisOption:RedisConn")]
    public string RedisConn { get; set; }

    [InjectConfiguration("RedisOption:SentinelAdders")]
    public List<string> SentinelAdders { get; set; }


    [InjectConfiguration("RedisOption:RedisCacheExpireSec")]
    public int RedisCacheExpireSec { get; set; } = 300;
}