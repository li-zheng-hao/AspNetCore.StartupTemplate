using System.Transactions;
using AspNetCore.StartUpTemplate.Utility;

namespace AspNetCore.StartUpTemplate.AOP;

/// <summary>
/// 把它添加到需要执行事务的Service层方法上，即可完成事务的操作。
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class UseTransactionAttribute:Attribute
{
    /// <summary>
    /// 忽略的异常
    /// </summary>
    public List<Type> IgnoreExceptions = new List<Type>() ;

    /// <summary>
    /// 事务隔离级别
    /// </summary>
    public IsolationLevel IsolationLevel;

    public UseTransactionAttribute(Propagation propagation=Propagation.Required,IsolationLevel isolationLevel=IsolationLevel.ReadCommitted,params Type[] ignoreExceptions)
    {
        this.Propagation = propagation;
        IsolationLevel = isolationLevel;
        var checkRes=ignoreExceptions?.Any(it => false== it.IsSubClassOrEqualEx(typeof(Exception)));
        if (checkRes != null&&checkRes== true)
        {
            throw new ArgumentException("传入的类型必须是Exception及派生类型");
        }
        IgnoreExceptions.AddRange(ignoreExceptions);
    }
    /// <summary>
    /// 事务传播行为
    /// </summary>
    public Propagation Propagation { get; set; }
}