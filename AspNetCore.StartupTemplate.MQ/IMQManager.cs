using System.Text;
using System.Text.Json.Serialization;
using AspNetCore.StartUpTemplate.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace AspNetCore.StartupTemplate.MQ;

/// <summary>
/// IOC单例使用,注意事项：
/// 1. 发布如果需要确认要在连接字符串上设置*publisherConfirms=true*
/// </summary>
public interface IMQManager
{

    void PushMessageDirect<T>(MessageModel<T> msg, string routingkey);

   
    void SubscribleDirect<T>(string routingkey, Action<MessageModel<T>> onMessage, string queuename = "",
        bool durable = false);

    void PushMessageTopic<T>(MessageModel<T> msg, string routingkey);

    void SubscribleTopic<T>(string routingkey, Action<MessageModel<T>> onMessage, string queuename = "",
        bool durable = false);
    
}