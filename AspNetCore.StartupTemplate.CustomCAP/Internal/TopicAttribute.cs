// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace DotNetCore.CAP.Internal
{
    /// <inheritdoc />
    /// <summary>
    /// 只用于rabbitmq
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public abstract class TopicAttribute : Attribute
    {
        
        protected TopicAttribute(string topicName,string exchangeName="",string queueName="", bool isPartial = false)
        {
            TopicName = topicName;
            IsPartial = isPartial;
            if(string.IsNullOrWhiteSpace(exchangeName)==false)
                ExchangeName = exchangeName;
            if(string.IsNullOrWhiteSpace(queueName)==false)
                Group = queueName;
        }
       
        /// <summary>
        /// 交换机名 不写由默认参数配置
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Topic名称
        /// </summary>
        public string TopicName { get; }

        /// <summary>
        /// Defines whether this attribute defines a topic subscription partial.
        /// The defined topic will be combined with a topic subscription defined on class level,
        /// which results for example in subscription on "class.method".
        /// </summary>
        public bool IsPartial { get; }

        /// <summary>
        /// 队列名
        /// Default group name is CapOptions setting.(Assembly name)
        /// kafka --> groups.id
        /// rabbit MQ --> queue.name
        /// </summary>
        public string Group { get; set; } = default!;
    }
}
