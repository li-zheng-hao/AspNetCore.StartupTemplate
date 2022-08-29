using AspNetCore.CacheOutput;
using AspNetCore.StartUpTemplate.Auth;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.StartUpTemplate.Webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<UserController> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserController(ILogger<UserController> logger, IMapper mapper, IUserRepository up)
    {
        _logger = logger;
        _mapper = mapper;
        _userRepository = up;
    }
    [CacheOutput(ClientTimeSpan = 100,ServerTimeSpan = 100)]
    [NeedAuth]
    [HttpGet("get")]
    public IEnumerable<WeatherForecast> Get(string id)
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .Where(it=>it.Summary.Contains(id))
            .ToArray();
    }
    [InvalidateCacheOutput("*")] // 清除所有本Controller下的缓存
    [HttpGet("tokentest")]
    public string TokenTest()
    {
        var res1123 = _userRepository.GetById("123");
        var tm = new UserData() { Id = "123", UserName = "lizhenghao" };
        var token = TokenHelper.CreateToken(tm);
        var res = TokenHelper.ResolveToken(token);
        return token;
    }
}