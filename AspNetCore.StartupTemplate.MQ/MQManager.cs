using AspNetCore.StartUpTemplate.Configuration;
using EasyNetQ;

namespace AspNetCore.StartupTemplate.MQ;

/// <summary>
/// IOC单例使用
/// 1. 发布确认在字符串上设置("host=localhost;*publisherConfirms=true*;timeout=10");
/// </summary>
public class MQManager:IDisposable 
{
    private static IBus _bus { get; set; }
    public MQManager() {
        _bus = RabbitHutch.CreateBus(AppSettingsConstVars.MQConnStr);
    }

    public void Publish()
    {
        _bus.PubSub.PublishAsync("1","topic");
    }

    public void Subscrible()
    {
        _bus.PubSub.Subscribe<object>("my_id", handlerOfXDotStar, x =>
        {
            x.
        });
    }

    private void handlerOfXDotStar<T>(T obj)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _bus.Dispose();
    }
}