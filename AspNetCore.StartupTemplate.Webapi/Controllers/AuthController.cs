using System.ComponentModel;
using AspNetCore.CacheOutput;
using AspNetCore.StartUpTemplate.Auth;
using AspNetCore.StartUpTemplate.Auth.HttpContextUser;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Contract;
using AspNetCore.StartUpTemplate.Contract.DTOs;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartupTemplate.Snowflake;
using AspNetCore.StartupTemplate.Snowflake.SnowFlake;
using AspNetCore.StartUpTemplate.Webapi.Startup;
using Dtmcli;
using FreeSql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.StartUpTemplate.Webapi.Controllers;

/// <summary>
/// 权限示例
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
[NeedAuth("admin")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHttpContextUser _httpContextUser;
    private readonly IHttpContextAccessor _accessor;

    public AuthController(ILogger<AuthController> logger,IHttpContextAccessor accessor,IHttpContextUser httpContextUser,IServiceProvider serviceProvider)
    {
        _accessor = accessor;
        _httpContextUser = httpContextUser;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    /// <summary>
    /// 获取当前登录的用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public UserData? GetCurUserData()
    {
        return _httpContextUser.UserData;
    }
    /// <summary>
    /// 根据参数生成不同角色的Token
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    [IgnoreAuth]
    [HttpGet]
    public string  GetToken(string role)
    {
        return TokenHelper.CreateToken(new UserData() { Role = role, Id = "123", UserName = "123" });
    }
    /// <summary>
    /// 未登录的情况下允许访问
    /// </summary>
    /// <returns></returns>
    [IgnoreAuth]
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