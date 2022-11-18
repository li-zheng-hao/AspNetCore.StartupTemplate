using System.ComponentModel;
using AspNetCore.CacheOutput;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Contract;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartupTemplate.CustomScheduler;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartupTemplate.Snowflake.SnowFlake;
using Dtmcli;
using FreeSql;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.StartUpTemplate.Webapi.Controllers;

/// <summary>
/// 定时任务示例
/// </summary>
[ApiController]
[Route("[controller]")]
public class SchedulerController : ControllerBase
{
    private readonly ILogger<SchedulerController> _logger;
    private readonly IFreeSql _freesql;
    private readonly SchedulerManager _schedulerManager;

    public SchedulerController(ILogger<SchedulerController> logger,IFreeSql freeSql,SchedulerManager schedulerManager)
    {
        _logger = logger;
        _freesql = freeSql;
        _schedulerManager = schedulerManager;
    }
    
    
    
    /// <summary>
    /// 创建定时任务
    /// </summary>
    [HttpPost("CreateTask")]
    public string CreateTask(string topic,int interval=5)
    {
        
        var (res,id)=_schedulerManager.AddTask(topic,interval);
        return $"创建完成,结果：{res}-id：{id}";
    }
   
}