using AspNetCore.StartUpTemplate.AOP;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartUpTemplate.Model;
using Autofac.Extras.DynamicProxy;

namespace AspNetCore.StartUpTemplate.Services;
public class UserService:BaseServices<Users>,IUserService
{
    private readonly IUserRepository _dal;
    private readonly ITestService _testService;

    public UserService(IUnitOfWork unitOfWork,IUserRepository userRepository,ITestService testService) : base(unitOfWork)
    {
        base.BaseRepo = userRepository;
        _dal = userRepository;
        _testService = testService;
    }
    [UseTransaction()]
    public void TestNestedTrans()
    {
        var user = new Users();
        user.Id = 12312312;
        user.UserName = "外部事务";
        var res=this.Insert(user);
        try
        {
            _testService.TestNestedTransError();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    [UseTransaction(Propagation.Nested)]
    public void TestNestedTransIn()
    {
        throw new Exception("diy");
        var res=_dal.GetList();

    }
}