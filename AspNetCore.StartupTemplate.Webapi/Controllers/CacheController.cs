using System.ComponentModel;
using AspNetCore.CacheOutput;
using AspNetCore.StartUpTemplate.Auth;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Contract;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartUpTemplate.Model;
using AspNetCore.StartupTemplate.Snowflake.SnowFlake;
using AspNetCore.StartUpTemplate.Webapi.Startup;
using AutoMapper;
using Dtmcli;
using FreeSql;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.StartUpTemplate.Webapi.Controllers;

/// <summary>
/// 通用示例
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class CacheController : ControllerBase
{
    private readonly ILogger<CacheController> _logger;
    private readonly IUserService _userService;
    private readonly IServiceProvider _serviceProvider;

    public CacheController(ILogger<CacheController> logger, IMapper mapper, IUserService us,IServiceProvider serviceProvider)
    {
        _logger = logger;
        _userService = us;
        _serviceProvider = serviceProvider;
    }
    
    /// <summary>
    /// 自定义的Cache缓存
    /// </summary>
    [HttpGet]
    public Users BatchQuery(string key="用户A")
    {
        return _userService.Query(key);
    }
}