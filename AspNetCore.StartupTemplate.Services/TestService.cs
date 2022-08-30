using AspNetCore.StartUpTemplate.AOP;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartUpTemplate.Model;
using Autofac.Extras.DynamicProxy;

namespace AspNetCore.StartUpTemplate.Services;
public class TestService:BaseServices<Users>,ITestService
{
    private readonly IUserRepository _dal;

    public TestService(IUnitOfWork unitOfWork,IUserRepository userRepository) : base(unitOfWork)
    {
        base.BaseRepo = userRepository;
        _dal = userRepository;
    }
    [UseTransaction()]
    public void TestNestedTransOk()
    {
        var res=_dal.GetList();
    }
    [UseTransaction(Propagation.Nested)]
    public void TestNestedTransError()
    {
        throw new Exception("diy");
        var res=_dal.GetList();

    }
}