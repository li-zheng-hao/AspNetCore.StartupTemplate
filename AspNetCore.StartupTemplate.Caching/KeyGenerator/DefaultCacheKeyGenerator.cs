using AspNetCore.StartupTemplate.CacheAsync.Configuration;
using AspNetCore.StartUpTemplate.Utility.Utils;
using MST.Infra.CacheProvider.Interface;
using MST.Infra.CacheProvider.KeyGenerator;
using Newtonsoft.Json;
using Rougamo.Context;

namespace AspNetCore.StartupTemplate.CacheAsync.KeyGenerator;

public class DefaultCacheKeyGenerator:ICacheKeyGenerator
{
    private readonly CacheOptions _cacheOptions;

    public DefaultCacheKeyGenerator(CacheOptions cacheOptions)
    {
        _cacheOptions = cacheOptions;
    }
    public string GeneratorKey(MethodContext methodContext)
    {
        string className = methodContext.TargetType.Name;
        string methodName = methodContext.Method.Name;
        List<object> argments = new();
        
        foreach (var methodContextArgument in methodContext.Arguments)
        {
            var paramType=methodContextArgument.GetType();
            if (typeof(ICacheKey).IsAssignableFrom(paramType))
            {
                var cacheKey=(methodContextArgument as ICacheKey)?.ToCacheKey();
                if (cacheKey != null && cacheKey.IsNotNullOrWhiteSpace())
                {
                    argments.Add(cacheKey);
                }
            }
            else
            {
                argments.Add(methodContextArgument);
            }
        }
        string param = string.Empty;
        if (argments.Count > 0)
        {
            string serializeString = JsonConvert.SerializeObject(argments, Formatting.None, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
            param = $":{EncryptHelper.Encrypt(serializeString)}";
        }
        return string.Concat($"{methodContext.TargetType.Namespace}:{className}:{methodName}", param);
    }
}