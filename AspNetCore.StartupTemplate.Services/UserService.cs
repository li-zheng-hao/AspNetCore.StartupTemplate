using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartUpTemplate.Model;
using FreeSql;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using AspNetCore.StartUpTemplate.Core.Transaction;

namespace AspNetCore.StartUpTemplate.Services;
public class UserService : IUserService
{
    private readonly ITestService _testService;
    private readonly ILogger<UserService> _logger;
    private readonly IBaseRepository<Users> _dal;

    public UserService(ILogger<UserService> logger, IBaseRepository<Users> userRepository, ITestService testService
    )
    {
        _logger = logger;
        _dal = userRepository;
        _testService = testService;
    }
    [Transactional]
    public void FuncA()
    {
        Users user = new Users();
        user.Id = Random.Shared.NextInt64();
        user.UserName = "FuncA" + Path.GetRandomFileName().ToLower();
        _dal.Insert(user);
        TestNestedTransError();
        throw new Exception("11");
    }
    [Transactional]
    public void TestNestedTransError()
    {
        Users user = new Users();
        user.Id = Random.Shared.NextInt64();
        user.UserName = "FuncA" + Path.GetRandomFileName().ToLower();
        _dal.Insert(user);
    }
    [Transactional]
    public void ChangeMoney(int userid, int number)
    {
        var u = _dal.Where(it => it.Id == userid).ToOne();
        u.Money += number;
        _dal.Update(u);
    }
    [Transactional]
    public void ChangeMoneyError(int userid, int number)
    {
        var u = _dal.Where(it => it.Id == userid).ToOne();
        u.Money += number;
        _dal.Update(u);
        throw new Exception("抛出异常");
    }
    [Transactional]
    public void InsertUserBatch()
    {
        List<Users> us = new List<Users>();
        for (int i = 0; i < 10000; i++)
        {
            Users u = new Users();
            u.Id = Random.Shared.NextInt64();
            u.UserName = "Path.GetRandomFileName()";
            u.Password = Path.GetRandomFileName();
            u.Address = Path.GetRandomFileName();
            u.Phone = Path.GetRandomFileName();
            u.Money = 444;
            us.Add(u);
        }

        _dal.Insert(us);
    }
    public void UpdateBatch()
    {
        var res = _dal.Orm.Update<Users>().Where(it => true).Set(it => it.Address, "modify address2").ExecuteAffrows();
        _logger.LogInformation($"修改了{res}行");
    }
    public List<Users> QueryAll()
    {
        var time = new Stopwatch();
        time.Restart();
        var res = _dal.Where(it => true).ToList();
        time.Stop();
        _logger.LogInformation($"全表查询{res.Count}个结果,时间{time.Elapsed}");
        return res;
    }
    public Users Query(string key)
    {
        int i = 1; // add this line
        var res = _dal.Where(it => it.UserName == key).First();
        return res;
    }
    public List<Users> PageQuery(int number, int size)
    {
        var time = new Stopwatch();
        time.Restart();
        var res = _dal.Where(it => true).Page(number, size).ToList();
        time.Stop();
        _logger.LogInformation($"分页查询{res.Count}个结果,时间{time.Elapsed}");
        return res;
    }

    public void JoinQuery()
    {
        var res = _dal.Orm.Select<Users, Orders>()
            .InnerJoin((a, b) => a.Id == b.UserId)
            .ToList((a, b) => new { a, b });
        Orders orders = new Orders();
        orders.Id = Random.Shared.NextInt64();
        orders.UserId = 100;
        _dal.Orm.Insert(orders).ExecuteAffrows();
        _logger.LogInformation($"查询到了{res.Count}");
    }


    [Transactional(Propagation = Propagation.Nested)]
    public void FuncB()
    {
        Users user = new Users();
        user.Id = Random.Shared.NextInt64();
        user.UserName = "FuncB" + Path.GetRandomFileName().ToLower();
        _dal.Orm.Insert(user);
        throw new Exception("1");

    }

    /// <summary>
    /// 测试CAP与FreeSql事务集成的示例
    /// </summary>
    [Transactional]
    public  Task CapWithFreeSqlTrans()
    {
     return Task.CompletedTask;  
    }
    /// <summary>
    /// 测试CAP与FreeSql事务集成的示例
    /// </summary>
    [Transactional]
    public async Task CapWithFreeSqlTransRollBack()
    {
        throw new Exception("随机抛出异常");
    }
}