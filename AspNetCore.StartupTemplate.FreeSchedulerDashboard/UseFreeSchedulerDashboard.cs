using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace AspNetCore.StartupTemplate.FreeSchedulerDashboard;


public class UseFreeSchedulerDashboard
{
    private readonly RequestDelegate _next;
    private readonly IApplicationBuilder _app;

    public UseFreeSchedulerDashboard(RequestDelegate next,IApplicationBuilder app)
    {
        _next = next;
        var ii=typeof(UseFreeSchedulerDashboard).Assembly;
        _app = app;
        
    }

    public async Task InvokeAsync(HttpContext context)
    {
        
        if(context.Request.Path.Value.Contains("assets"))
            context.Request.Path = context.Request.Path.Value.Replace("assets", "freedashboard/assets");
      
        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }
}

public static class RequestCultureMiddlewareExtensions
{
    public static IApplicationBuilder UseFreeSchedulerDashboard(
        this IApplicationBuilder builder)
    {
        
         builder.UseMiddleware<UseFreeSchedulerDashboard>(builder);
        builder.UseStaticFiles(new StaticFileOptions()
        {
            // FileProvider = new PhysicalFileProvider("D:/AspNetCore.StartupTemplate/content/AspNetCore.StartupTemplate.FreeSchedulerDashboard/dashboard"),
            FileProvider = new EmbeddedFileProvider(typeof(UseFreeSchedulerDashboard).Assembly,"AspNetCore.StartupTemplate.FreeSchedulerDashboard.dashboard"), 
            RequestPath = "/freedashboard"
        });
        return builder;
    }
}