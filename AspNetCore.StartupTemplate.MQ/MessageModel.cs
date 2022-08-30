using System.Text;
using Newtonsoft.Json;

namespace AspNetCore.StartupTemplate.MQ;

public class MessageModel<T>
{
    public MessageModel(T t)
    {
        Message = t;
    }
    /// <summary>
    /// 消息内容
    /// </summary>
    public T Message { get; set; }

    /// <summary>
    /// 消息唯一ID 用于计算重试次数
    /// </summary>
    public string MsgId { get; set; } = Guid.NewGuid().ToString("N");
    public byte[] ToBytes()
    {
        var json = JsonConvert.SerializeObject(this);
        return Encoding.UTF8.GetBytes(json);
    } 
}