// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Concurrent;

namespace AspNetCore.StartupTemplate.CustomRbMQ
{
    public abstract class MQTransactionBase : IMQTransaction
    {
        private readonly IDispatcher _dispatcher;

        private readonly ConcurrentQueue<MediumMessage> _bufferList;

        protected MQTransactionBase(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _bufferList = new ConcurrentQueue<MediumMessage>();
        }

        public bool AutoCommit { get; set; }

        public virtual object? DbTransaction { get; set; }

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

        public abstract void Commit();

        public abstract Task CommitAsync(CancellationToken cancellationToken = default);

        public abstract void Rollback();

        public abstract Task RollbackAsync(CancellationToken cancellationToken = default);

        public abstract void Dispose();
    }
}
