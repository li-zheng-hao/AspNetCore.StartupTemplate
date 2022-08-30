using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartUpTemplate.Model;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace AspNetCore.StartUpTemplate.Services;
public class UserService:BaseServices<Users>,IUserService
{
    private readonly IUserRepository _dal;
    private readonly ITestService _testService;
    private readonly ILogger<UserService> _logger;

    public UserService(ILogger<UserService> logger,IUnitOfWork unitOfWork,IUserRepository userRepository,ITestService testService) : base(unitOfWork)
    {
        _logger = logger;
        base.BaseRepo = userRepository;
        _dal = userRepository;
        _testService = testService;
    }
    
}