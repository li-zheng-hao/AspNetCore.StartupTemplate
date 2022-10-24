using System.Net;
using System.Security.Claims;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Contract;
using AspNetCore.StartUpTemplate.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCore.StartUpTemplate.Auth;

/// <summary>
/// 自定义身份验证
/// </summary>
public class NeedAuthAttribute : Attribute, IAuthorizationFilter
{
    public string[] Roles { get; set; }

    public NeedAuthAttribute(params string[] roles)
    {
        Roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor.ControllerOrMethodHasAttribute<IgnoreAuthAttribute>())
        {
            return;
        }
        else if (context.HttpContext.Request.Headers.ContainsKey("Authorization") == false)
        {
            Failed(context, ResponseCode.AUTH_ERROR, "身份验证未通过");
        }
        else
        {
            var token = (string)context.HttpContext.Request.Headers["Authorization"];
            if (token.StartsWith("Bearer"))
            {
                token = token.Split(" ")[1];
            }

            var (isPassed, model) = TokenHelper.ValidateToken(token);
            if (isPassed == 1)
            {
                Failed(context, ResponseCode.AUTH_ERROR, "身份令牌超时");
            }
            else if (isPassed > 1)
            {
                Failed(context, ResponseCode.AUTH_ERROR, "身份验证未通过");
            }
            else
            {
                if ((Roles.Length > 0 && Roles.Any(it => it == model!.UserData.Role)) == false)
                {
                    Failed(context,ResponseCode.AUTH_FORBID,"权限不足", StatusCodes.Status403Forbidden);
                }
            }
        }
    }


    private void Failed(AuthorizationFilterContext context, int code, string message,
        int statusCode = StatusCodes.Status401Unauthorized)
    {
        context.HttpContext.Response.StatusCode = statusCode;
        context.Result = new JsonResult(new ResponseResult()
        {
            Code = code,
            Msg = message
        });
    }
}