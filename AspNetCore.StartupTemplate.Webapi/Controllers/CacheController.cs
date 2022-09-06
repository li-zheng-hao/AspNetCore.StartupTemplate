using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartUpTemplate.Model;
using AutoMapper;
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

    public CacheController(ILogger<CacheController> logger, IMapper mapper, IUserService us, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _userService = us;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 自定义的Cache缓存
    /// </summary>
    [HttpGet]
    public Users BatchQuery(string key = "用户A")
    {
        return _userService.Query(key);
    }
    /// <summary>
    /// 自定义的Cache缓存
    /// </summary>
    [HttpGet]
    public dynamic PageQuery()
    {
        var res= _userService.PageQuery(10, 10);
        return res;
    }
    /// <summary>
    /// 清除自定义缓存
    /// </summary>
    [HttpGet]
    public void ClearCache()
    {
        _userService.UpdateBatch();
    }
}