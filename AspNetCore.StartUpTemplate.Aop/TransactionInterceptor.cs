using AspNetCore.StartUpTemplate.IRepository;
using Castle.Core.Internal;
using Castle.DynamicProxy;

namespace AspNetCore.StartUpTemplate.AOP;

/// <summary>
/// 事务处理AOP
/// </summary>
public class TransactionInterceptor : IInterceptor
{
    private UseTransactionAttribute _useTransactionAttribute { get; set; }
    /// <summary>
    /// 通过属性注入
    /// </summary>
    public IUnitOfWork UnitOfWork { get; set; }
    public void Intercept(IInvocation invocation)
    {
        System.Diagnostics.Debug.WriteLine("你正在调用方法 \"{0}\"  参数是 {1}... ",
            invocation.Method.Name,              
            string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));
        var attribute = invocation.Method.GetAttribute<UseTransactionAttribute>();
        if (attribute != null)
        {
            _useTransactionAttribute = attribute;
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
            // todo 此处需要判断是否为要忽略的异常类型
            UnitOfWork.CommitTran();
            throw;
        }
        UnitOfWork.CommitTran();

        System.Diagnostics.Debug.WriteLine("方法执行完毕，返回结果：{0}", invocation.ReturnValue);
    }
}