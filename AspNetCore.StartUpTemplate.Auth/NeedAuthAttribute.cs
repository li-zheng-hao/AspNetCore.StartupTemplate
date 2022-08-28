using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCore.StartUpTemplate.Auth;

/// <summary>
/// 自定义身份验证
/// </summary>
public class NeedAuthAttribute :Attribute,IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Request.Headers.ContainsKey("Authorization")==false)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Result=new JsonResult(new ResponseResult()
            {
                Code = ResponseCode.AUTH_OUT_OF_DATE_ERROR,
                Msg = "身份验证未通过"
            });
        }
        else
        {
            var token=(string)context.HttpContext.Request.Headers["Authorization"];
            if (token.StartsWith("Bearer"))
            {
                token=token.Split(" ")[1];
            }
            var isPassed=TokenHelper.ValidateToken(token);
            if (isPassed == 1)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result=new JsonResult(new ResponseResult()
                {
                    Code = ResponseCode.AUTH_OUT_OF_DATE_ERROR,
                    Msg = "身份令牌超时"
                });
            }
            else if(isPassed>1)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result=new JsonResult(new ResponseResult()
                {
                    Code = ResponseCode.AUTH_ERROR,
                    Msg = "身份验证未通过"
                });
            }
        }
        
    }

    
}