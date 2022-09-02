using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartUpTemplate.Model;
using AspNetCore.StartupTemplate.Snowflake.SnowFlake;
using Autofac.Extras.DynamicProxy;
using FreeSql;
using Microsoft.Extensions.Logging;

namespace AspNetCore.StartUpTemplate.Services;
public class UserService:IUserService
{
    private readonly ITestService _testService;
    private readonly ILogger<UserService> _logger;
    private readonly IBaseRepository<Users> _dal;

    public UserService(ILogger<UserService> logger,IBaseRepository<Users> userRepository,ITestService testService) 
    {
        _logger = logger;
        _dal = userRepository;
        _testService = testService;
    }
    [Transactional]
    public void FuncA()
    {
        Users user = new Users();
        user.Id = IocHelper.Resolve<ISnowflakeIdMaker>().NextId();
        user.UserName = "FuncA" + Path.GetRandomFileName().ToLower();
        _dal.Insert(user);
        try
        {
            _testService.TestNestedTransError();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    [Transactional]
    public void ChangeMoney(int userid,int number)
    {
        var u=_dal.Where(it => it.Id == userid).ToOne();
        u.Money += number;
        _dal.Update(u);
    }
    [Transactional]
    public void ChangeMoneyError(int userid,int number)
    {
        var u=_dal.Where(it => it.Id == userid).ToOne();
        u.Money += number;
        _dal.Update(u);
        throw new Exception("抛出异常");
    }


    [Transactional(Propagation = Propagation.Nested)]
    public void FuncB()
    {
        Users user = new Users();
        user.Id = IocHelper.Resolve<ISnowflakeIdMaker>().NextId();
        user.UserName = "FuncB" + Path.GetRandomFileName().ToLower();
        _dal.Orm.Insert(user);
        throw new Exception("1");

    }
}