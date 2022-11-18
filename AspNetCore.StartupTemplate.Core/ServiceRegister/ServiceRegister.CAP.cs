using AspNetCore.StartUpTemplate.Configuration.Option;
using DotNetCore.CAP.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AspNetCore.StartUpTemplate.Core.ServiceRegister;

public  static partial class ServiceRegister
{
    public static IServiceCollection AddCustomCAP(this IServiceCollection serviceCollection,IConfiguration configuration)
    {
        var mysqlOption = configuration.GetSection("Mysql").Get<MysqlOption>();
        var rabbitMqOption = configuration.GetSection("RabbitMQ").Get<RabbitMQOption>();
        serviceCollection.AddCap(x =>
        {
            x.UseDashboard();
            x.UseMySql(mysqlOption.ConnectionString);
            x.UseRabbitMQ(it =>
            {
                it.HostName = rabbitMqOption.HostName;
                it.Port =rabbitMqOption.Port ;
                it.UserName = rabbitMqOption.UserName;
                it.Password = rabbitMqOption.Password;
                it.VirtualHost = rabbitMqOption.VirtualHost;
            });
            x.FailedRetryCount = 5;
            x.FailedThresholdCallback = failed =>
            {
                Log.Logger.Error($@"A message of type {failed.MessageType} failed after executing {x.FailedRetryCount} several times, 
                        requiring manual troubleshooting. Message name: {failed.Message.GetName()}");
                // todo 短信/邮件通知异常
            };
        });
        return serviceCollection;
    }
}