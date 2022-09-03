namespace AspNetCore.StartupTemplate.CustomScheduler;

/// <summary>
/// 标志当前方法为一个定时任务的处理函数
/// </summary>
[AttributeUsage(AttributeTargets.Method,AllowMultiple = true,Inherited = true)]
public class SchedulerTaskAttribute:Attribute
{
    public SchedulerTaskAttribute(string topic)
    {
        Topic = topic;
    }
    public string Topic { get; set; }
}