using System.Reflection;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartupTemplate.CustomScheduler.Tasks;
using AspNetCore.StartUpTemplate.Utility;
using FreeScheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCore.StartupTemplate.CustomScheduler;

public class TaskManager:ITaskManager
{
    private List<Tuple<string, MethodInfo>> _taskList = new List<Tuple<string, MethodInfo>>();
    private readonly ILogger<TaskManager> _logger;
    private readonly IServiceProvider _sp;

    public TaskManager(ILogger<TaskManager> logger,IServiceProvider serviceProvider)
    {
        _logger = logger;
        ScanTasks();
        _sp = serviceProvider;
    }
    /// <summary>
    /// 扫描当前程序集下所有的任务
    /// </summary>
    private void ScanTasks()
    {
        var alltypes=typeof(TaskManager).Assembly.GetTypes();
        foreach (var classType in alltypes)
        {
            var methodInfos = classType.GetMethods().Where(it=>it.MethodHasAttribute<SchedulerTaskAttribute>());
            methodInfos.All(method =>
            {
                var topic=method.GetCustomAttribute<SchedulerTaskAttribute>()?.Topic;
                if (string.IsNullOrWhiteSpace(topic))
                    throw new Exception("无法添加topic为空字符串的任务,请检查方法特性标记是否正确");
                _taskList.Add(new Tuple<string, MethodInfo>(topic,method));
                return true;
            });
        }
        _logger.LogInformation($"扫描到了{_taskList.Count}个任务方法");
    }
    public bool InvokeTask(TaskInfo taskInfo)
    {
        if (_taskList.Any(it => it.Item1 == taskInfo.Topic) == false)
        {
            _logger.LogWarning($"未找到{taskInfo.Topic}相关的定时任务处理函数,请确认是否存在遗漏");
            return false;
        }
        // 方法可以存在多个
        var tasks=_taskList.Where(it => it.Item1 == taskInfo.Topic).ToList();
        foreach (var task in tasks)
        {
            // 注意，这里必须每个方法开一个生命周期范围，因为不是从管道进来的，不会创建子生命周期范围
            using (var scope = _sp.CreateScope())
            {
                var classIntance= (IDisposable) scope.ServiceProvider.GetService(task.Item2.DeclaringType);
                TransactionalAttribute.SetServiceProvider(_sp);
                task.Item2.Invoke(classIntance,new object?[]{ taskInfo});
            }
           
        }
        
        return true;
    }

 

    

    public bool StopTask(string topic)
    {
        return true;
    }
}

public interface ITaskManager
{
    public bool InvokeTask(TaskInfo taskInfo);

    public bool StopTask(string topic);
    
}