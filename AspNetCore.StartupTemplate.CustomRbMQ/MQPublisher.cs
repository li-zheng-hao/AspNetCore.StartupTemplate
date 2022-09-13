using AspNetCore.StartupTemplate.Snowflake.SnowFlake;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNetCore.StartupTemplate.CustomRbMQ;

internal class MQPublisher : IMQPublisher
{
    private readonly IDispatcher _dispatcher;
    private readonly IDataStorage _storage;
    private readonly MQOptions _mqOptions;
    private readonly ISnowflakeIdMaker _snowflakeIdMaker;
    private readonly ILogger<MQPublisher> _logger;

    // ReSharper disable once InconsistentNaming

    public MQPublisher(IServiceProvider service)
    {
        ServiceProvider = service;
        _dispatcher = service.GetRequiredService<IDispatcher>();
        _storage = service.GetRequiredService<IDataStorage>();
        _logger = service.GetRequiredService<ILogger<MQPublisher>>();
        _snowflakeIdMaker = service.GetRequiredService<ISnowflakeIdMaker>();
        _mqOptions = service.GetRequiredService<IOptions<MQOptions>>().Value;
        Transaction = new AsyncLocal<IMQTransaction>();
    }

    public IServiceProvider ServiceProvider { get; }

    public AsyncLocal<IMQTransaction> Transaction { get; }

    public Task PublishAsync<T>(string name, T? value, IDictionary<string, string?> headers,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(() => Publish(name, value, headers), cancellationToken);
    }

    public Task PublishAsync<T>(string name, T? value, string? callbackName = null,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(() => Publish(name, value, callbackName), cancellationToken);
    }

    public void Publish<T>(string name, T? value, string? callbackName = null)
    {
        var header = new Dictionary<string, string?>
        {
            // todo 头部
        };

        Publish(name, value, header);
    }

    public void Publish<T>(string name, T? value, IDictionary<string, string?> headers)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (!string.IsNullOrEmpty(_mqOptions.TopicNamePrefix))
        {
            name = $"{_mqOptions.TopicNamePrefix}.{name}";
        }

        if (!headers.ContainsKey(Headers.MessageId))
        {
            var messageId = _snowflakeIdMaker.NextId().ToString();
            headers.Add(Headers.MessageId, messageId);
        }

        if (!headers.ContainsKey(Headers.CorrelationId))
        {
            headers.Add(Headers.CorrelationId, headers[Headers.MessageId]);
            headers.Add(Headers.CorrelationSequence, 0.ToString());
        }

        headers.Add(Headers.MessageName, name);
        headers.Add(Headers.Type, typeof(T).Name);
        headers.Add(Headers.SentTime, DateTimeOffset.Now.ToString());

        var message = new Message(headers, value);

        long? tracingTimestamp = null;
        try
        {
            tracingTimestamp = TracingBefore(message);

            if (Transaction.Value?.DbTransaction == null)
            {
                var mediumMessage = _storage.StoreMessage(name, message);

                TracingAfter(tracingTimestamp, message);

                _dispatcher.EnqueueToPublish(mediumMessage);
            }
            else
            {
                var transaction = (MQTransaction)Transaction.Value;

                var mediumMessage = _storage.StoreMessage(name, message, transaction.DbTransaction);

                TracingAfter(tracingTimestamp, message);

                transaction.AddToSent(mediumMessage);

                if (transaction.AutoCommit)
                {
                    transaction.Commit();
                }
            }
        }
        catch (Exception e)
        {
            TracingError(tracingTimestamp, message, e);

            throw;
        }
    }

    #region tracing

    private long? TracingBefore(Message message)
    {
        var timestamp=DateTime.Now.Ticks;
        _logger.LogTrace("MQ发送消息 {0} 时间戳{1}", message, timestamp.ToString());
        return timestamp;
    }

    private void TracingAfter(long? tracingTimestamp, Message message)
    {
        _logger.LogTrace("MQ发送消息完成 {0} 时间戳{1}", message, tracingTimestamp.ToString());

    }

    private void TracingError(long? tracingTimestamp, Message message, Exception ex)
    {
        _logger.LogError("MQ发送消息异常 {0} 时间戳{1} 异常信息{2}", message, tracingTimestamp.ToString(),ex);
    }

    #endregion
}