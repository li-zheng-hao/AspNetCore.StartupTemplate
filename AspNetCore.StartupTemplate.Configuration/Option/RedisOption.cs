using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace AspNetCore.StartUpTemplate.Configuration.Option;

[RegisterService(ServiceLifetime.Singleton)]
public class RedisOption
{
    [InjectConfiguration("Redis:ServiceName")]
    public string ServiceName { get; set; }

    [InjectConfiguration("Redis:Password")]
    public string Password { get; set; }

    [InjectConfiguration("Redis:RedisConn")]
    public string RedisConn { get; set; }

    [InjectConfiguration("Redis:SentinelAdders")]
    public string[] SentinelAdders { get; set; }


    [InjectConfiguration("Redis:RedisCacheExpireSec")]
    public int RedisCacheExpireSec { get; set; } = 300;
}