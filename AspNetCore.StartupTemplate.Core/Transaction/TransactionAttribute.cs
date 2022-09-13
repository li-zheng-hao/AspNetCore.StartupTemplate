using System.Data;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using Rougamo.Context;
using Serilog;
using Serilog.Core;

namespace AspNetCore.StartUpTemplate.Core;


[AttributeUsage(AttributeTargets.Method)]
public class TransactionalAttribute : Rougamo.MoAttribute
{
    public Propagation Propagation { get; set; } = Propagation.Required;
    public IsolationLevel IsolationLevel { get => m_IsolationLevel.Value; set => m_IsolationLevel = value; }
    IsolationLevel? m_IsolationLevel;

    static AsyncLocal<IServiceProvider> m_ServiceProvider = new AsyncLocal<IServiceProvider>();
    public static void SetServiceProvider(IServiceProvider serviceProvider) => m_ServiceProvider.Value = serviceProvider;

    IUnitOfWork _uow;
    public override void OnEntry(MethodContext context)
    {
        Log.Debug("进入 Transactional事务切面");
        var uowManager = m_ServiceProvider.Value.GetService<UnitOfWorkManager>();
        _uow = uowManager.Begin(this.Propagation, this.m_IsolationLevel);
        Log.Debug($"当前事务的guid为{_uow.Orm.Ado.Identifier}");
        // todo 获取一个mqtransaction并标记开启了事务
    }
    public override void OnExit(MethodContext context)
    {
        if (typeof(Task).IsAssignableFrom(context.RealReturnType))
            ((Task)context.ReturnValue).ContinueWith(t => _OnExit());
        else _OnExit();
        // todo 这里获取当前的mq 推送实例，每个mqtransaction AsyncLocal形式 都是scoped，因此这里手动提交或刷新
        // 每个mqtransaction判断是否开启了事务,如果开启则跳过处理，否则提交开始发消息
        // 开启了事务在下面提交或取消提交
        void _OnExit()
        {
            try
            {
                if (context.Exception == null) 
                    _uow.Commit();
                else
                {
                    Log.Error("UnitofWorkManager 切面 回滚 ");
                    _uow.Rollback();
                }
            }
            finally
            {
                _uow.Dispose();
            }
        }
        
        Log.Debug("退出 Transactional事务切面");
    }
}