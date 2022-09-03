using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetCore.StartUpTemplate.Filter;

/// <summary>
/// 隐藏接口，不生成到swagger文档展示
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class SwaggerIgnoreAttribute : Attribute { }
public class SwaggerIgnoreFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var ignoreApis = context.ApiDescriptions.Where(wh => wh.ActionDescriptor.EndpointMetadata.Any(any => any is SwaggerIgnoreAttribute));
        if (ignoreApis != null)
        {
            foreach (var ignoreApi in ignoreApis)
            {
                swaggerDoc.Paths.Remove("/" + ignoreApi.RelativePath);
            }
        }
 
    }
}