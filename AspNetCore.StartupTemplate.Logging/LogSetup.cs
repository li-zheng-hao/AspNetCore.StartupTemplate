using AspNetCore.StartUpTemplate.Configuration;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace AspNetCore.StartupTemplate;

public class LogSetup
{
    public static Logger Setup(IConfiguration configuration)
    {
        const string OUTPUT_TEMPLATE = "[{Level}] {ENV} {Timestamp:yyyy-MM-dd HH:mm:ss.fff} {SourceContext} <{ThreadId}>  {Message:lj}{NewLine}{Exception}";
        var logger= new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .Enrich.WithProperty("ENV",configuration["Env"])
            .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: OUTPUT_TEMPLATE)
                .WriteTo.File("logs/applog_.log"
                    , rollingInterval: RollingInterval.Day
                    , outputTemplate: OUTPUT_TEMPLATE)
                .CreateLogger();
        Log.Logger = logger;
        return logger;
    }
}