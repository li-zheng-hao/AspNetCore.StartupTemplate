using AspNetCore.CacheOutput;
using AspNetCore.StartUpTemplate.Auth;
using AspNetCore.StartUpTemplate.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace {{NameSpacePrefix}}.Webapi.Controllers;


/// <summary>
/// {{ModelDescription}}
///</summary>
[Description("{{ModelDescription}}")]
[Route("api/[controller]/[action]")]
[ApiController]
[NeedAuth]
public class {{ModelClassName}}Controller : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly I{{ModelClassName}}Services _{{ModelClassName}}Services;
    private readonly IMapper _mapper;
    /// <summary>
    /// 构造函数
    ///</summary>
    public {{ModelClassName}}Controller(IWebHostEnvironment webHostEnvironment
        ,I{{ModelClassName}}Services {{ModelClassName}}Services,IMapper mapper
        )
    {
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
        _{{ModelClassName}}Services = {{ModelClassName}}Services;
    }
}
