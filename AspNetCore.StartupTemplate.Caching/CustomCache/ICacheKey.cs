namespace MST.Infra.CacheProvider.Interface;

/// <summary>
/// 定制化转换成Key
/// </summary>
public interface ICacheKey
{
    /// <summary>
    /// 自定义转换成的string,用于缓存时生成对应参数的Key
    /// </summary>
    /// <returns></returns>
    string ToCacheKey();
}