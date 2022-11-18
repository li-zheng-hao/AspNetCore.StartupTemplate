using AspNetCore.StartUpTemplate.Configuration.Option;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;

namespace AspNetCore.StartUpTemplate.Core.Serilog;

public class LogSetup
{
    public static Logger InitSeialog(IConfiguration configuration)
    {
        var elasticSearchOption = configuration.GetSection("ElasticSearch").Get<ElasticSearchOption>();
        const string OUTPUT_TEMPLATE =
            "[{Level}] {EnvironmentName} {Timestamp:yyyy-MM-dd HH:mm:ss.fff} {SourceContext} <{ThreadId}>  {Message:lj}{NewLine}{Exception}";
        var config = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console(outputTemplate: OUTPUT_TEMPLATE)
            .WriteTo.File("logs/applog_.log"
                , rollingInterval: RollingInterval.Day
                , outputTemplate: OUTPUT_TEMPLATE);
        // 如果有elasticsearch则写入
        if (elasticSearchOption is not null && elasticSearchOption.Url.IsNotNullOrWhiteSpace())
            config.WriteTo.Elasticsearch(
                new ElasticsearchSinkOptions(new Uri(elasticSearchOption.Url)) // for the docker-compose implementation
                    {
                        AutoRegisterTemplate = true,
                        OverwriteTemplate = true,
                        DetectElasticsearchVersion = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                        // NumberOfReplicas = 1,
                        // NumberOfShards = 2,
                        // BufferBaseFilename = "logs/buffer",
                        // RegisterTemplateFailure = RegisterTemplateRecovery.FailSink,
                        // FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                        // EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                           // EmitEventFailureHandling.WriteToFailureSink |
                                           // EmitEventFailureHandling.RaiseCallback,
                        // FailureSink = new FileSink("logs/fail-{Date}.txt", new JsonFormatter(), null, null)
                    });

        var logger = config.CreateLogger();
        Log.Logger = logger;
        return logger;
    }
}