using System.ComponentModel;
using AspNetCore.CacheOutput;
using AspNetCore.StartUpTemplate.Configuration.Option;
using AspNetCore.StartUpTemplate.Contract;
using AspNetCore.StartUpTemplate.Contract.DTOs;
using AspNetCore.StartUpTemplate.Core.Jwt;
using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartUpTemplate.Model;
using AspNetCore.StartupTemplate.Snowflake;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.StartUpTemplate.Webapi.Controllers;

/// <summary>
/// 通用示例
/// </summary>
[ApiController]
[Route("[controller]/[action]")]

public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly IServiceProvider _serviceProvider;
    private readonly RedisOption _redisOption;

    public UserController(RedisOption redisOption,ILogger<UserController> logger, IUserService us,IServiceProvider serviceProvider)
    {
        _redisOption = redisOption;
        _logger = logger;
        _userService = us;
        _serviceProvider = serviceProvider;
        
    }
    
    
    /// <summary>
    /// 缓存示例
    /// </summary>
    /// <param name="id">随机传入的ID</param>
    /// <returns></returns>
    [Description("缓存")]
    [CacheOutput(ClientTimeSpan = 100,ServerTimeSpan = 100)]
    [HttpGet]
    public string  Get(string id)
    {
        return id;
    }
    /// <summary>
    /// 权限验证示例
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public string  AuthTest(string id)
    {
        return id;
    }
    [AllowAnonymous]
    [HttpGet]
    public string  WithoutAuthTest(string id)
    {
        return id;
    }
    /// <summary>
    /// 清除所有缓存示例 + Token生成
    /// </summary>
    /// <returns></returns>
    [InvalidateCacheOutput("*")] 
    [HttpGet]
    public string TokenTest()
    {
        return "123";
    }

    /// <summary>
    /// 获取雪花ID
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public string GetSnowFlake()
    {
        return _serviceProvider.GetService<SnowflakeGenerator>().NextId().ToString();
    }
    /// <summary>
    /// 注解式事务示例
    /// </summary>
    [HttpGet]
    public void TransSample()
    {
        _userService.FuncA();
    }
    
    /// <summary>
    /// 批量插入1w
    /// </summary>
    [HttpGet]
    public void BatchInsert()
    {
        _userService.InsertUserBatch();
    }
    /// <summary>
    /// 批量更新
    /// </summary>
    [HttpGet]
    public void BatchUpdate()
    {
        _userService.UpdateBatch();
    }
    /// <summary>
    /// 全表查询
    /// </summary>
    [HttpGet]
    public void BatchQuery()
    {
        _userService.QueryAll();
        try
        {
            throw new Exception("测试自定义异常输出到elasticsearch");
        }
        catch (Exception e)
        {
            _logger.LogError(e,$"自定义异常{e}");
        }
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    [HttpGet]
    public void PageQuery(int number=100,int size=100)
    {
        var users=_userService.PageQuery(number,size);
    }
    
    /// <summary>
    /// 连表查询
    /// </summary>
    [HttpGet]
    public void JoinQuery()
    {
        _userService.JoinQuery();
    }



}