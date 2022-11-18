using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Configuration.GlobalConfig;

namespace AspNetCore.StartUpTemplate.Contract;

public class ResponseResult
{
    /// <summary>
    ///     返回编码
    /// </summary>
    public int Code { get; set; } = ResponseCode.SUCCESS;
    /// <summary>
    /// 返回消息
    /// </summary>
    public string Msg { get; set; }
    /// <summary>
    /// 返回数据
    /// </summary>
    public object Data { get; set; }
    /// <summary>
    ///     状态码
    /// </summary>
    public bool Status { get; set; } = true;

    public static ResponseResult Failure(string msg,int code=ResponseCode.OTHER_ERROR)
    {
        return new ResponseResult() { Code = code, Msg = msg,Status=false };
    }
    public static ResponseResult Success(object data)
    {
        return new ResponseResult() { Data=data };
    }
}