using System.Data;
using System.Reflection;
using DotNetCore.CAP;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using Rougamo.Context;
using Serilog;

namespace AspNetCore.StartUpTemplate.Core.Transaction;


[AttributeUsage(AttributeTargets.Method,AllowMultiple = false,Inherited = false)]
public class TransactionalAttribute : Rougamo.MoAttribute
{
    public Propagation Propagation { get; set; } = Propagation.Required;
    /// <summary>
    /// 是否使用CAP事务,true的话CAP也加入事务,默认false
    /// </summary>
    public bool UseCapTrans { get; set; } = false;
    public IsolationLevel IsolationLevel { get => m_IsolationLevel.Value; set => m_IsolationLevel = value; }
    IsolationLevel? m_IsolationLevel;

    static AsyncLocal<IServiceProvider> m_ServiceProvider = new AsyncLocal<IServiceProvider>();
    public static void SetServiceProvider(IServiceProvider serviceProvider) => m_ServiceProvider.Value = serviceProvider;

    IUnitOfWork _uow;
    private ICapPublisher _capPublisher;
    private MySqlCapTransaction _capTransaction;
    private bool NeedDisposeCapTrans { get; set; } = true;
    public override void OnEntry(MethodContext context)
    {
        Log.Debug("进入 Transactional事务切面");
        var uowManager = m_ServiceProvider!.Value!.GetService<UnitOfWorkManager>()!;
        _uow = uowManager.Begin(this.Propagation, this.m_IsolationLevel);
        var trans=_uow.GetOrBeginTransaction();

        #region CAP相关
        if (UseCapTrans)
        {
            // 这里接入cap的事务 并且设置cap的自动提交为false
            // ICapPublisher是单例的,里面的是事务是AsyncLocal存储 
            _capPublisher = m_ServiceProvider!.Value!.GetService<ICapPublisher>()!;
            if (_capPublisher!.Transaction.Value is  null)
            {
                _capTransaction = ActivatorUtilities.CreateInstance<MySqlCapTransaction>(m_ServiceProvider.Value);;
                _capTransaction.AutoCommit = false;
                _capTransaction.DbTransaction = trans;
                _capPublisher.Transaction.Value = _capTransaction;
            }
        }
        #endregion
        Log.Debug($"当前事务的guid为{_uow.Orm.Ado.Identifier}");
    }
    public override void OnExit(MethodContext context)
    {
        if (typeof(Task).IsAssignableFrom(context.RealReturnType))
            ((Task)context.ReturnValue).ContinueWith(t => _OnExit());
        else _OnExit();
        void _OnExit()
        {
            try
            {
                if (context.Exception == null)
                {
                    Log.Error("UnitOfWorkManager 切面 提交 ");
                
                    _uow.Commit();
                    if (UseCapTrans)
                    {
                        var uowName=_uow.GetType().Name;
                        // 假提交，CAP先不推消息
                        if (uowName == "UnitOfWorkVirtual" || uowName == "UnitOfWorkNothing")
                        {
                            NeedDisposeCapTrans = false;
                            return;

                        }
                        _capTransaction?.GetType().GetMethod("Flush", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(_capTransaction, null);
                    }
                }
                else
                {
                    Log.Error("UnitOfWorkManager 切面 回滚 ");
                    _uow.Rollback();
                }
            }
            finally
            {
                if(NeedDisposeCapTrans&&UseCapTrans)
                    _capTransaction?.Dispose();

                _uow.Dispose();
            }
        }
        
        Log.Debug("退出 Transactional事务切面");
    }
}