using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AspNetCore.StartUpTemplate.Configuration;

/// <summary>
/// 配置文件格式化
/// </summary>
public class AppSettingsConstVars
{
    #region 全局配置======================================================================
    public static readonly string EnvironmentMode = AppSettingsHelper.GetContent("Env");

    #endregion

    #region 数据库================================================================================
    /// <summary>
    /// 获取数据库连接字符串
    /// </summary>
    public static readonly string DbConnection = AppSettingsHelper.GetContent("Mysql", "ConnectionString");
   
    #endregion

    #region RabbitMQ================================================================================
    public static readonly string MQDirectExchangeName = AppSettingsHelper.GetContent("RabbitMQ:Direct:ExchangeName");
    public static readonly string MQTopicExchangeName = AppSettingsHelper.GetContent("RabbitMQ:Topic:ExchangeName");
    public static readonly string MQHostName = AppSettingsHelper.GetContent("RabbitMQ:HostName");
    public static readonly int MQPort = AppSettingsHelper.GetContentInteger("RabbitMQ:Port");
    public static readonly string MQVirtualHost = AppSettingsHelper.GetContent("RabbitMQ:VirtualHost");
    public static readonly string MQUserName = AppSettingsHelper.GetContent("RabbitMQ:UserName");
    public static readonly string MQPassword = AppSettingsHelper.GetContent("RabbitMQ:Password");
    #endregion

    #region redis================================================================================

    /// <summary>
    /// 获取redis连接字符串
    /// </summary>
    //public static readonly string RedisConfigConnectionString = AppSettingsHelper.GetContent("RedisConfig", "ConnectionString");
    public static readonly string RedisConn = AppSettingsHelper.GetContent("Redis","RedisConn");
    public static readonly string RedisServiceName = AppSettingsHelper.GetContent("Redis","ServiceName");
    public static readonly string RedisPassword = AppSettingsHelper.GetContent("Redis","Password");
    public static readonly int RedisExpireSec = AppSettingsHelper.GetContentInteger("Redis","RedisCacheExpireSec");
    public static readonly List<string> RedisSentinelAdders = AppSettingsHelper.GetContentList<string>("Redis","SentinelAdders");

    #endregion


    #region Jwt授权配置================================================================================

    public static readonly string JwtConfigSecretKey = AppSettingsHelper.GetContent("Jwt", "Key");
    public static readonly string JwtConfigIssuer = AppSettingsHelper.GetContent("Jwt", "Issuer");
    public static readonly string JwtConfigAudience = AppSettingsHelper.GetContent("Jwt", "Audience");
    #endregion

    #region DTM相关配置=========================================================================

    public static readonly string DtmUrl = AppSettingsHelper.GetContent("Dtm:DtmUrl");
    public static readonly string BusiUrl = AppSettingsHelper.GetContent("Dtm:BusiUrl");
    public static readonly string DtmBarrierTableName = AppSettingsHelper.GetContent("Dtm:DtmBarrierTableName");

    

    #endregion
}

