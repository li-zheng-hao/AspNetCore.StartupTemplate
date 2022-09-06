namespace AspNetCore.StartUpTemplate.Core.Cache
{
    /// <summary>
    /// 标记了本特性的方法在ClearChache特性中会根据配置进行清除
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class NeedClearCacheAttribute : Attribute
    {
    }
}
