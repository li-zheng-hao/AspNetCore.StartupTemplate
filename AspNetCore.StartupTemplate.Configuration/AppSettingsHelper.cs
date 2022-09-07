namespace AspNetCore.StartUpTemplate.Configuration;

using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

/// <summary>
/// 获取Appsettings配置信息
/// </summary>
public class AppSettingsHelper
{
    private static IConfiguration Configuration { get; set; }

    public AppSettingsHelper(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// 封装要操作的字符
    /// AppSettingsHelper.GetContent(new string[] { "JwtConfig", "SecretKey" });
    /// </summary>
    /// <param name="sections">节点配置</param>
    /// <returns></returns>
    public static string GetContent(params string[] sections)
    {
        try
        {

            if (sections.Any())
            {
                return Configuration[string.Join(":", sections)];
            }
        }
        catch (Exception) { }

        return "";
    }
    /// <summary>
    /// 封装要操作的字符
    /// AppSettingsHelper.GetContent(new string[] { "JwtConfig", "SecretKey" });
    /// </summary>
    /// <param name="sections">节点配置</param>
    /// <returns></returns>
    public static int GetContentInteger(params string[] sections)
    {

        var str=GetContent(sections);
        var convert=int.TryParse(str, out int result);
        if (!convert) throw new Exception($"{string.Join(":", sections)} 配置读取失败,请检查配置文件");
        return result;
    }
    /// <summary>
    /// 封装要操作的字符
    /// AppSettingsHelper.GetContent(new string[] { "JwtConfig", "SecretKey" });
    /// </summary>
    /// <param name="sections">节点配置</param>
    /// <returns></returns>
    public static List<T> GetContentList<T>(params string[] sections)
    {

        if (sections.Any())
        {
            List<T> result = new List<T>();
            var children= Configuration.GetSection(string.Join(":", sections)).GetChildren();
            foreach (var item in children)
            {
                result.Add( (T) Convert.ChangeType(item.Value, typeof(T)));
            }
            return result;
        }

        return null;
    }
}
