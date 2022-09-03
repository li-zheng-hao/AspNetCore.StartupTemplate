using FreeScheduler;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AspNetCore.StartupTemplate.CustomScheduler;

class MyTaskHandler :  FreeScheduler.TaskHandlers.FreeSqlHandler
{
    public MyTaskHandler(IFreeSql fsql) : base(fsql) { }
    
    public override void OnExecuting(FreeScheduler.Scheduler scheduler, TaskInfo task)
    {
        base.OnExecuting(scheduler, task);
        Log.Information($"进入了 当前任务信息{ task.Id} {task.Topic}");
        
        // todo 通过反射获取当前程序集下面所有标记了SchedulerTask特性的方法，然后通过反射执行该方法
    }
}
public class CustomScheduler
{
    public CustomScheduler(IFreeSql freeSql)
    {
        _freeSql = freeSql;
        var tsk=new MyTaskHandler(freeSql);
        scheduler= new FreeScheduler.Scheduler(tsk);
    }

    FreeScheduler.Scheduler scheduler;
    private readonly IFreeSql _freeSql;
    private readonly ILogger<CustomScheduler> _logger;

    /// <summary>
    /// 新增自定义时间任务
    /// </summary>
    /// <param name="topic"></param>
    public (bool,string) AddTask(string topic,int intervalSecond=10)
    {
        var result = _freeSql.Ado.QuerySingle<dynamic>("select * from freescheduler_task where topic = @topic", new { topic });
        if (result is null)
        {
            var id=scheduler.AddTask(topic, "", -1, intervalSecond);
            Console.WriteLine($"新增任务-id:{id}-topic:{topic}-间隔:{intervalSecond}");
            return (true, id);
        }

        return (false, string.Empty);
    }
}