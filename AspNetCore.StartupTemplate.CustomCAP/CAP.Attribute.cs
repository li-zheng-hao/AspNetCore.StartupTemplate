// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DotNetCore.CAP.Internal;

// ReSharper disable once CheckNamespace
namespace DotNetCore.CAP
{
    public class CapSubscribeAttribute : TopicAttribute
    {
        /// <summary>
        /// RabbitMQ Topic模式
        /// </summary>
        /// <param name="topicName">主题 必填</param>
        /// <param name="exchangeName">交换机名称 选填</param>
        /// <param name="queueName">队列名 选填</param>
        /// <param name="isPartial">是否要包含类名 选填</param>
        public CapSubscribeAttribute(string topicName,string exchangeName="",string queueName="", bool isPartial = false)
            : base(topicName, exchangeName,queueName,isPartial)
        {

        }

        public override string ToString()
        {
            return $"topic:{TopicName}-exchange:{ExchangeName}-group:{Group}";
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromCapAttribute : Attribute
    {
       
    }

    public class CapHeader : ReadOnlyDictionary<string, string?>
    {
        public CapHeader(IDictionary<string, string?> dictionary) : base(dictionary)
        {

        }
    }
}