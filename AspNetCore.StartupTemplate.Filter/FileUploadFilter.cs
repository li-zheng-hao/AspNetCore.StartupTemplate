using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AspNetCore.StartUpTemplate.Filter;
/// <summary>
/// 用于在Swagger中上传文件
/// </summary>
public class FileUploadFilter : IOperationFilter
{

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        //判断上传文件的类型，只有上传的类型是IFormCollection的才进行重写。
        if (context.ApiDescription.ActionDescriptor.Parameters.Any(w => w.ParameterType == typeof(IFormCollection)))
        {
            Dictionary<string, OpenApiSchema> schema = new Dictionary<string, OpenApiSchema>();
            schema["fileName"] = new OpenApiSchema { Description = "Select file", Type = "string", Format = "binary" };
            Dictionary<string, OpenApiMediaType> content = new Dictionary<string, OpenApiMediaType>();
            content["multipart/form-data"] = new OpenApiMediaType { Schema = new OpenApiSchema { Type = "object", Properties = schema } };
            operation.RequestBody = new OpenApiRequestBody() { Content = content };
        }
    }
}
