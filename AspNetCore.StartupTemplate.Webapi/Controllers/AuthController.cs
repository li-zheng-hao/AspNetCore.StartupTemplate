using System.Security.Claims;
using AspNetCore.StartUpTemplate.Core.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quickwire.Attributes;

namespace AspNetCore.StartUpTemplate.Webapi.Controllers;

/// <summary>
/// 权限示例
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
[RegisterService(ServiceLifetime.Scoped)]
[InjectAllInitOnlyProperties]
public class AuthController : ControllerBase
{
    public ILogger<AuthController> _logger { get; init; }
    public IServiceProvider _serviceProvider { get; init; }
    public IHttpContextAccessor _accessor { get; init; }
    public JwtTokenManager _jwtTokenManager { get; init; }

   
    /// <summary>
    /// 根据参数生成不同角色的Token
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpGet]
    public string GetToken(string role) => _jwtTokenManager.GenerateToken(new Claim[]
    {
        new Claim(ClaimTypes.Name, "test"),
        new Claim(ClaimTypes.Role, role)
    });

    /// <summary>
    /// 未登录的情况下允许访问
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public bool WithoutAuthTest()
    {
        return true;
    }

    /// <summary>
    /// 测试需要权限的接口
    /// </summary>
    /// <returns></returns>
    [HttpGet, Authorize]
    public string AuthTest()
    {
        return "OK";
    }

    /// <summary>
    /// 该接口需要为admin角色才能访问
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public bool AdminRequireTest()
    {
        return true;
    }
}