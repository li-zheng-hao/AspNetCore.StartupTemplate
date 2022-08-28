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

    #region redis================================================================================

    /// <summary>
    /// 获取redis连接字符串
    /// </summary>
    //public static readonly string RedisConfigConnectionString = AppSettingsHelper.GetContent("RedisConfig", "ConnectionString");


    #endregion


    #region Jwt授权配置================================================================================

    public static readonly string JwtConfigSecretKey = AppSettingsHelper.GetContent("Jwt", "Key");
    public static readonly string JwtConfigIssuer = AppSettingsHelper.GetContent("Jwt", "Issuer");
    public static readonly string JwtConfigAudience = AppSettingsHelper.GetContent("Jwt", "Audience");
    #endregion

}

