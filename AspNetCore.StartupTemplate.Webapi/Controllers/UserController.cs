using System.ComponentModel;
using AspNetCore.CacheOutput;
using AspNetCore.StartUpTemplate.Auth;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartupTemplate.Snowflake.SnowFlake;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.StartUpTemplate.Webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IMapper mapper, IUserService us)
    {
        _logger = logger;
        _mapper = mapper;
        _userService = us;
    }
    /// <summary>
    /// 缓存+权限验证示例
    /// </summary>
    /// <param name="id">随机传入的ID</param>
    /// <returns></returns>
    [Description("缓存+权限验证示例")]
    [CacheOutput(ClientTimeSpan = 100,ServerTimeSpan = 100)]
    [NeedAuth]
    [HttpGet("get")]
    public string  Get(string id)
    {
        return id;
    }
    /// <summary>
    /// 清除所有缓存示例 + Token生成
    /// </summary>
    /// <returns></returns>
    [InvalidateCacheOutput("*")] 
    [HttpGet("tokentest")]
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
    [HttpGet("GetSnowFlake")]
    public string GetSnowFlake()
    {
        return IocHelper.Resolve<ISnowflakeIdMaker>().NextId().ToString();
    }
    /// <summary>
    /// 注解式事务示例
    /// </summary>
    [HttpGet("transsample")]
    public void TransSample()
    {
        _userService.FuncA();
        
    }
}