using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace AspNetCore.StartUpTemplate.Configuration.Option;

[RegisterService(ServiceLifetime.Singleton)]
public class JwtOption
{
    /// <summary>
    /// 对称加密密钥
    /// </summary>
    [InjectConfiguration("Jwt:SecretKey")]
    public string SecretKey { get; set; }

    [InjectConfiguration("Jwt:Audience")]
    public string Audience { get; set; }

    [InjectConfiguration("Jwt:Issuer")]
    public string Issuer { get; set; }

    /// <summary>
    /// Token的过期时间 单位: 秒
    /// </summary>
    // [InjectConfiguration("Jwt:ExpireSeconds")]
    public int ExpireSeconds { get; set; } = 60 * 60;
}