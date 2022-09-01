using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartUpTemplate.Model;
using AspNetCore.StartupTemplate.Snowflake.SnowFlake;
using FreeSql;
using Microsoft.Extensions.Logging;

namespace AspNetCore.StartUpTemplate.Services;
public class TestService:ITestService
{
    private readonly IBaseRepository<Test> _dal;
    private readonly ILogger<TestService> _logger;

    public TestService(ILogger<TestService> logger,IBaseRepository<Test>  testRepository)
    {
        _logger = logger;
        _dal = testRepository;
    }
  
    [Transactional]
    public void TestNestedTransError()
    {
        Test test = new Test();
        test.Id = IocHelper.Resolve<ISnowflakeIdMaker>().NextId();
        test.UserName = "FuncB" + Path.GetRandomFileName().ToLower();
        _dal.Insert(test);
        throw new Exception("11");
    }
    
    
    /// <summary>
    /// Nested等于Spring中的RequireNew
    /// </summary>
    [Transactional(Propagation = Propagation.Nested)]
    public void TestNestedTransOk()
    {
        Users user = new Users();
        user.Id = IocHelper.Resolve<ISnowflakeIdMaker>().NextId();
        user.UserName = "FuncB" + Path.GetRandomFileName().ToLower();
        _dal.Orm.Insert(user);
    }
}