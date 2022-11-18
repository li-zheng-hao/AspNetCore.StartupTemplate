using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace AspNetCore.StartUpTemplate.Configuration.Option;

[RegisterService(ServiceLifetime.Singleton)]
public class RabbitMQOption
{
    [InjectConfiguration("RabbitMQ:HostName")]
    public string HostName { get; set; }

    [InjectConfiguration("RabbitMQ:Port")] public int Port { get; set; }

    [InjectConfiguration("RabbitMQ:VirtualHost")]
    public string VirtualHost { get; set; }

    [InjectConfiguration("RabbitMQ:UserName")]
    public string UserName { get; set; }

    [InjectConfiguration("RabbitMQ:Password")]
    public string Password { get; set; }

    [InjectConfiguration("RabbitMQ:ExchangeName")]
    public string ExchangeName { get; set; }
}