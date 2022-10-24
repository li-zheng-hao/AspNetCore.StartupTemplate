using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.Model;
using FreeScheduler;
using Serilog;

namespace AspNetCore.StartupTemplate.CustomScheduler.Tasks;

public class DemoTask:IDisposable
{
    private readonly IFreeSql _freeSql;

    public DemoTask(IFreeSql freesql)
    {
        _freeSql = freesql;
    }
    [Transactional]
    [SchedulerTask("test")]
    public void UpdateUsers(TaskInfo taskInfo)
    {
        var user=_freeSql.Select<Users>().ToOne();
        Log.Information($"调用到了UpdateUsers方法 查询到用户名 {user.Id}");

    }

    public void Dispose()
    {
        // 测试Transient对象是否被销毁，防止内存泄漏
        Log.Warning("DemoTask被销毁了！！！");
    }
}