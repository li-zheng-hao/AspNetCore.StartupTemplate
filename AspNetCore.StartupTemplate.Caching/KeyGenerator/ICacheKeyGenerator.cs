using Rougamo.Context;

namespace MST.Infra.CacheProvider.KeyGenerator;

public interface ICacheKeyGenerator
{
    
    /// <summary>
    /// 根据方法信息生成缓存Key
    /// </summary>
    /// <param name="methodContext"></param>
    /// <returns></returns>
    string GeneratorKey(MethodContext methodContext);
}