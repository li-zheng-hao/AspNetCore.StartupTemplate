using AspNetCore.StartUpTemplate.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;

namespace AspNetCore.StartUpTemplate.Webapi.Startup;

public class LogSetup
{
    public static Logger InitSeialog(IConfiguration configuration)
    {
        const string OUTPUT_TEMPLATE =
            "[{Level}] {ENV} {Timestamp:yyyy-MM-dd HH:mm:ss.fff} {SourceContext} <{ThreadId}>  {Message:lj}{NewLine}{Exception}";
        var config = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .Enrich.WithProperty("ENV", configuration["Env"])
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console(outputTemplate: OUTPUT_TEMPLATE)
            .WriteTo.File("logs/applog_.log"
                , rollingInterval: RollingInterval.Day
                , outputTemplate: OUTPUT_TEMPLATE);
        // 如果有elasticsearch则写入
        if (String.IsNullOrWhiteSpace(configuration["ElasticSearchUrl"])==false)
            config.WriteTo.Elasticsearch(
                new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearchUrl"])) // for the docker-compose implementation
                    {
                        AutoRegisterTemplate = true,
                        OverwriteTemplate = true,
                        DetectElasticsearchVersion = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                        NumberOfReplicas = 1,
                        NumberOfShards = 2,
                        // BufferBaseFilename = "logs/buffer",
                        // RegisterTemplateFailure = RegisterTemplateRecovery.FailSink,
                        FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                           EmitEventFailureHandling.WriteToFailureSink |
                                           EmitEventFailureHandling.RaiseCallback,
                        FailureSink = new FileSink("logs/fail-{Date}.txt", new JsonFormatter(), null, null)
                    });

        var logger = config.CreateLogger();
        Log.Logger = logger;
        return logger;
    }
}