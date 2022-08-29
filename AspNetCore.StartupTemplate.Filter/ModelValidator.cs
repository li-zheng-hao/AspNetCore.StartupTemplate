

using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetCore.StartUpTemplate.Filter;

/// <summary>
/// 请求验证错误处理
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public class ModelValidator : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext actionContext)
    {
        var modelState = actionContext.ModelState;
        if (!modelState.IsValid)
        {
            var baseResult = new ResponseResult()
            {
                Status = false,
                Code = ResponseCode.PARAM_ERROR,
                Msg = "请提交必要的参数",
            };
            List<string> errors = new List<string>();
            foreach (var key in modelState.Keys)
            {
                var state = modelState[key];
                if (state.Errors.Any())
                {
                    errors.Add( $"{key} -  { state.Errors.FirstOrDefault().ErrorMessage}");
                }
            }
            baseResult.Data = errors;
            actionContext.Result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(baseResult),
                ContentType = "application/json"
            };
        }
    }
}
