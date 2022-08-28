namespace AspNetCore.StartUpTemplate.Configuration;
public static class ResponseCode
{
    /// <summary>
    /// 正常
    /// </summary>
    public const int SUCCESS = 200;
    /// <summary>
    /// 参数错误
    /// </summary>
    public const int PARAM_ERROR = 601;
    /// <summary>
    /// 其他
    /// </summary>
    public const int OTHER_ERROR = 500;
    /// <summary>
    /// token未通过校验
    /// </summary>
    public const int AUTH_ERROR = 401;
    /// <summary>
    /// token超时
    /// </summary>
    public const int AUTH_OUT_OF_DATE_ERROR = 402;

}
