using System.Data;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using Rougamo.Context;

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
        var uowManager = m_ServiceProvider.Value.GetService<UnitOfWorkManager>();
        _uow = uowManager.Begin(this.Propagation, this.m_IsolationLevel);
    }
    public override void OnExit(MethodContext context)
    {
        Console.Write(_uow.Orm.Ado.Identifier.ToString());
        if (typeof(Task).IsAssignableFrom(context.RealReturnType))
            ((Task)context.ReturnValue).ContinueWith(t => _OnExit());
        else _OnExit();

        void _OnExit()
        {
            try
            {
                if (context.Exception == null) 
                    _uow.Commit();
                else
                {
                    Console.Write("回滚");
                    _uow.Rollback();
                }
            }
            finally
            {
                _uow.Dispose();
            }
        }
    }
}