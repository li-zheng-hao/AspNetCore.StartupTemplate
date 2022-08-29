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

    public UseTransactionAttribute(params Type[] ignoreExceptions)
    {
        
        var checkRes=ignoreExceptions?.Any(it => false== it.IsSubClassOrEqualEx(typeof(Exception)));
        if (checkRes != null&&checkRes== true)
        {
            throw new ArgumentException("传入的类型必须是Exception及派生类型");
        }
        IgnoreExceptions.AddRange(ignoreExceptions);
    }
}