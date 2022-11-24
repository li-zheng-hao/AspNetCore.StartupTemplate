using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.StartupTemplate.Job;
public static class JobMiddlewareExtension
{
    public static IApplicationBuilder UseJobMiddleware(this IApplicationBuilder builder)
    {
        builder.UseHangfireDashboard();
        HangfireJobService.Start();
        return builder;
    }
}