using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.Utility;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace AspNetCore.StartUpTemplate.AOP;

/// <summary>
/// 事务处理AOP
/// </summary>
public class TransactionInterceptor : IInterceptor
{
    private readonly ILogger<TransactionInterceptor> _logger;
    private UseTransactionAttribute _useTransactionAttribute { get; set; }
    /// <summary>
    /// 通过属性注入
    /// </summary>
    public IUnitOfWork UnitOfWork { get; set; }
    public TransactionInterceptor(ILogger<TransactionInterceptor> logger)
    {
        _logger = logger;
    }
   
    public void Intercept(IInvocation invocation)
    {
#if DEBUG  
        Console.WriteLine("你正在调用方法 \"{0}\"  参数是 {1}... ",
            invocation.Method.Name,              
            string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));
#endif

        // 加在interface上
        var attributeOnInterface = invocation.Method.GetAttribute<UseTransactionAttribute>();
        // 加在了实现类上
        var attributeOnImpl = invocation.MethodInvocationTarget.GetAttribute<UseTransactionAttribute>();
        if (attributeOnInterface != null)
        {
            _useTransactionAttribute = attributeOnInterface;
            UnitOfWork.BeginTran();
        }else if (attributeOnImpl != null)
        {
            _useTransactionAttribute = attributeOnImpl;
            UnitOfWork.BeginTran();
        }

        try
        {
            //在被拦截的方法执行完毕后 继续执行           
            invocation.Proceed();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            var checkRes=this.IsIgnoreException(e);
            if(checkRes)
                UnitOfWork.CommitTran();
            else
            {
                UnitOfWork.RollbackTran();
            }
            _logger.LogError("AOP层捕获到异常",e);
            throw;
        }
        if(UnitOfWork.IsUsingTransaction())
            UnitOfWork.CommitTran();
#if DEBUG  
        Console.WriteLine("方法执行完毕，返回结果：{0}", invocation.ReturnValue);
#endif
    }


    private bool IsIgnoreException(Exception ex)
    {
        var list=_useTransactionAttribute.IgnoreExceptions;
        
        return list.Any(it => ex.GetType().IsSubClassOrEqualEx(it));
    }
}