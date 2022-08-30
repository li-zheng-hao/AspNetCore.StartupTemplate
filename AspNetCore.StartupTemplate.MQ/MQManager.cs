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
public class MQManager:IDisposable 
{
    /// <summary>
    /// 生命周期与应用生命周期一致
    /// </summary>
    private readonly IConnection _conn;
    public MQManager() {
        // todo 通过AppSettings获取
        ConnectionFactory factory = new ConnectionFactory();
        factory.UserName = "user";
        factory.Password = "pwd";
        factory.VirtualHost = "/";
        factory.HostName = "hostname";
        _conn = factory.CreateConnection();
        InitExchange();
        InitDurableQueue();
    }
    

    public async Task PushMessageDirect<T>(MessageModel<T> msg, string routingkey) 
    {
        var channel=_conn.CreateModel();
        var prop=channel.CreateBasicProperties();
        channel.ConfirmSelect();
        channel.BasicPublish(exchange: AppSettingsConstVars.MQDirectExchangeName,
            routingKey: routingkey,
            basicProperties: null,
            body: msg.ToBytes());
        channel.WaitForConfirms();
    }

    /// <summary>
    /// 订阅消息 Direct模式
    /// </summary>
    /// <param name="routingkey"></param>
    /// <param name="onMessage">消息到了之后的处理事件</param>
    /// <param name="queuename">队列名 默认空时由rabbitmq随机生成</param>
    /// <param name="durable">队列是否为持久化的 默认false</param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="Exception"></exception>
    public void SubscribleDirect<T>(string routingkey,Action<MessageModel<T>> onMessage,string queuename="",bool durable=false)
    {
        var channel=_conn.CreateModel();
        try
        {
            channel.QueueDeclare(queue: queuename,
                durable: durable,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(queue: queuename,
                exchange: AppSettingsConstVars.MQDirectExchangeName,
                routingKey: routingkey);
        }
        catch
        {
            //异常以后2秒再次重新订阅
            Thread.Sleep(2000);
            SubscribleDirect<T>(routingkey, onMessage,queuename);
            return;
        }

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender,e)=> 
        {
            try
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                MessageModel<T> t = JsonConvert.DeserializeObject<MessageModel<T>>(message);
                onMessage(t);
                channel.BasicAck(e.DeliveryTag, true);
            }
            catch (Exception exception)
            {
                // todo 重试N次并超过次数后再异常
                channel.BasicNack(e.DeliveryTag, false, false);
                throw ;
            }
        };
        // 需要手动ack保证消息不丢失
        channel.BasicConsume(queue: queuename,
            autoAck: false,
            consumer: consumer);
    }
    public void PushMessageTopic<T>(MessageModel<T> msg, string routingkey) 
    {
        var channel=_conn.CreateModel();
        var prop=channel.CreateBasicProperties();
        channel.ConfirmSelect();
        channel.BasicPublish(exchange: AppSettingsConstVars.MQTopicExchangeName,
            routingKey: routingkey,
            basicProperties: null,
            body: msg.ToBytes());
        channel.WaitForConfirms();
    }

    /// <summary>
    /// 订阅消息 Topic模式
    /// </summary>
    /// <param name="routingkey"></param>
    /// <param name="onMessage"></param>
    /// <param name="queuename"></param>
    /// <param name="durable">队列是否为持久化的 默认为false</param>
    /// <typeparam name="T"></typeparam>
    public void SubscribleTopic<T>(string routingkey,Action<MessageModel<T>> onMessage,string queuename="",bool durable=false)
    {
        var channel=_conn.CreateModel();
        try
        {
            channel.QueueDeclare(queue: queuename,
                durable: durable,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(queue: queuename,
                exchange: AppSettingsConstVars.MQTopicExchangeName,
                routingKey: routingkey);
        }
        catch
        {
            //异常以后2秒再次重新订阅
            Thread.Sleep(2000);
            SubscribleDirect<T>(routingkey, onMessage,queuename);
            return;
        }

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender,e)=> 
        {
            try
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                MessageModel<T> t = JsonConvert.DeserializeObject<MessageModel<T>>(message);
                onMessage(t);
                channel.BasicAck(e.DeliveryTag, true);
            }
            catch (Exception exception)
            {
                // todo 重试N次并超过次数后再异常
                channel.BasicNack(e.DeliveryTag, false, false);
                throw ;
            }
        };
        // 需要手动ack保证消息不丢失
        channel.BasicConsume(queue: queuename,
            autoAck: false,
            consumer: consumer);
    }
    public void Dispose()
    {
        _conn.Dispose();
    }

    #region 私有方法 初始化永久队列和交换机

    /// <summary>
    /// 创建持久化的队列
    /// </summary>
    private void InitDurableQueue()
    {
    }
    /// <summary>
    /// 创建交换机
    /// </summary>
    private void InitExchange()
    {
        var channel=_conn.CreateModel();
        // todo 配置死信队列
        var dic=new Dictionary<string, object>();
        channel.ExchangeDeclare(AppSettingsConstVars.MQDirectExchangeName,"direct",true,false,dic);
        channel.ExchangeDeclare(AppSettingsConstVars.MQTopicExchangeName,"topic",true,false,dic);
    }
    #endregion

    
}