using System.Collections.Concurrent;

namespace AspNetCore.StartupTemplate.CustomRbMQ;

public class MQTransaction:IMQTransaction
{
    private readonly IDispatcher _dispatcher;

    private readonly ConcurrentQueue<MediumMessage> _bufferList;

    protected MQTransaction(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _bufferList = new ConcurrentQueue<MediumMessage>();
    }

    public bool AutoCommit { get; set; }

    public virtual object? DbTransaction { get; set; }
    public void Commit()
    {
        throw new NotImplementedException();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Rollback()
    {
        throw new NotImplementedException();
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    protected internal virtual void AddToSent(MediumMessage msg)
    {
        _bufferList.Enqueue(msg);
    }

    protected virtual void Flush()
    {
        while (!_bufferList.IsEmpty)
        {
            _bufferList.TryDequeue(out var message);

            _dispatcher.EnqueueToPublish(message);
        }
    }

  
}