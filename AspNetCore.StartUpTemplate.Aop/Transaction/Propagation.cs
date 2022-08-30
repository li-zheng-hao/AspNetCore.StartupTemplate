using System.ComponentModel;

namespace AspNetCore.StartUpTemplate.AOP;
/// <summary>
/// 事务传播行为
/// </summary>
public enum Propagation
{
    [Description("有事务加事务，无事务创建事务")]
    Required=0, 
    [Description("必须创建新事务")]
    RequireNew=1,
    [Description("有事务加入,无事务")]
    Supports=2,
    [Description("有事务加事务，无事务异常")]
    Mandatory=3,
    [Description("有事务异常")]
    Never=4,
    [Description("嵌套事务,设置SavePoint")]
    Nested=5
}