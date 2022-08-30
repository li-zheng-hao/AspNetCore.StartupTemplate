using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartUpTemplate.Model;
using Autofac.Extras.DynamicProxy;
using SqlSugar;

namespace AspNetCore.StartUpTemplate.Services;
public class TestService:BaseServices<Users>,ITestService
{
    private readonly IUserRepository _dal;

    public TestService(IUnitOfWork unitOfWork,IUserRepository userRepository) : base(unitOfWork)
    {
        base.BaseRepo = userRepository;
        _dal = userRepository;
    }
    public void TestNestedTransOk()
    {
        var res=_dal.GetList();
    }
    public void TestNestedTransError()
    {
        Users u = new Users();
        u.UserName = Path.GetRandomFileName();
        u.Id = SnowFlakeSingle.Instance.NextId();
        Insert(u);
        throw new Exception("ex");
    }
}