using AspNetCore.StartUpTemplate.Core.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.StartUpTemplate.Webapi.Controllers;

/// <summary>
/// 权限示例
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHttpContextAccessor _accessor;

    public AuthController(ILogger<AuthController> logger,IHttpContextAccessor accessor,IServiceProvider serviceProvider)
    {
        _accessor = accessor;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    
    /// <summary>
    /// 根据参数生成不同角色的Token
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpGet]
    public string  GetToken(string role)
    {
        // 正常开发时Role应该从数据库中获取,存在一个问题,当用户角色更新时,需要重新登录才行
        return "123";
    }
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
    /// 该接口需要为admin角色才能访问
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public bool AdminRequireTest()
    {
        return true;
    }
   
}