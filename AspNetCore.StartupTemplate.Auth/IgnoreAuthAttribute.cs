namespace AspNetCore.StartUpTemplate.Auth;

/// <summary>
/// 使用了此特性的接口不需要检测权限
/// </summary>
[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple = false,Inherited = true)]
public class IgnoreAuthAttribute : Attribute
{
    
}