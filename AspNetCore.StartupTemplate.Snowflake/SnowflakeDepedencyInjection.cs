using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartupTemplate.Snowflake;

public static class SnowflakeDepedencyInjection
{
    public static IServiceCollection AddSnowflakeGenerator(this IServiceCollection service,
        Action<SnowflakeOption>? action=null)
    {
        var opt = new SnowflakeOption();
        action?.Invoke(opt);
        service.AddSingleton(opt);
        service.AddSingleton<SnowflakeGenerator>();
        service.AddSingleton<SnowflakeWorkIdManager>();
        service.AddHostedService<SnowflakeBackgroundServices>();
        return service;
    }
}

public class SnowflakeOption
{
    /// <summary>
    /// 未设置的情况下从redis中读取
    /// </summary>
    public byte? WorkId { get; set; }
    /// <summary>
    /// 默认值10 1024个节点
    /// </summary>
    public byte WorkerIdBitLength { get; set; } = 10;
    /// <summary>
    /// 默认值6 序列位长度
    /// </summary>
    public byte SeqBitLength { get; set; } = 7;
}