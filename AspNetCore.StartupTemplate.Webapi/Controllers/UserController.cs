using System.ComponentModel;
using AspNetCore.CacheOutput;
using AspNetCore.StartUpTemplate.Auth;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Contract;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.IService;
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
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IServiceProvider _serviceProvider;

    public UserController(ILogger<UserController> logger, IMapper mapper, IUserService us,IServiceProvider serviceProvider)
    {
        _logger = logger;
        _mapper = mapper;
        _userService = us;
        _serviceProvider = serviceProvider;
        
    }
    
    
    /// <summary>
    /// 缓存+权限验证示例
    /// </summary>
    /// <param name="id">随机传入的ID</param>
    /// <returns></returns>
    [Description("缓存+权限验证示例")]
    [CacheOutput(ClientTimeSpan = 100,ServerTimeSpan = 100)]
    [NeedAuth]
    [HttpGet]
    public string  Get(string id)
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
        var tm = new UserData() { Id = "123", UserName = "lizhenghao" };
        var token = TokenHelper.CreateToken(tm);
        var res = TokenHelper.ResolveToken(token);
        return token;
    }

    /// <summary>
    /// 获取雪花ID
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public string GetSnowFlake()
    {
        return _serviceProvider.GetService<ISnowflakeIdMaker>().NextId().ToString();
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
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    [HttpGet]
    public void PageQuery(int number=100,int size=100)
    {
        _userService.PageQuery(number,size);
    }
}