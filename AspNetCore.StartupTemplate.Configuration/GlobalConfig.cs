namespace AspNetCore.StartUpTemplate.Configuration;

public  class GlobalConfig
{
    public static GlobalConfig Instance { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Jwt Jwt { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Env { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Mysql Mysql { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Redis Redis { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public RabbitMQ RabbitMQ { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Dtm Dtm { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string ElasticSearchUrl { get; set; }
}

public class Jwt
{
    /// <summary>
    /// 
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Audience { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Issuer { get; set; }
}

public class Mysql
{
    /// <summary>
    /// 
    /// </summary>
    public string ConnectionString { get; set; }
}

public class Redis
{
    /// <summary>
    /// 
    /// </summary>
    public string ServiceName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string RedisConn { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List <string > SentinelAdders { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int RedisCacheExpireSec { get; set; }
}


public class RabbitMQ
{
    /// <summary>
    /// 
    /// </summary>
    public string HostName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Port { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string VirtualHost { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Password { get; set; }
    public string ExchangeName { get; set; }
}

public class Dtm
{
    /// <summary>
    /// 
    /// </summary>
    public string DtmUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string BusiUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string DtmBarrierTableName { get; set; }
}

