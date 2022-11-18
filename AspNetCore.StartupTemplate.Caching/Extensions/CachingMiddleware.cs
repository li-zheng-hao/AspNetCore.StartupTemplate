using AspNetCore.StartupTemplate.CacheAsync.Interceptor;
using Microsoft.AspNetCore.Http;
using MST.Infra.CacheProvider.Interceptor;

namespace AspNetCore.StartupTemplate.CacheAsync.Extensions;

public class CachingMiddleware
{
    private readonly RequestDelegate _next;
    public CachingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        CachingEnableAttribute.SetServiceProvider(context.RequestServices);
        CacheClearAttribute.SetServiceProvider(context.RequestServices);
        await _next(context);
    }
}