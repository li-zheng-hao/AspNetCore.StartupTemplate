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
    public List<Exception> IgnoreExceptions = new List<Exception>();

    public UseTransactionAttribute(Exception[] ignoreExceptions=null)
    {
        IgnoreExceptions.AddRange(ignoreExceptions);
    }
}