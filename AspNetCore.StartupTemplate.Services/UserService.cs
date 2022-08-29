using AspNetCore.StartUpTemplate.AOP;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartUpTemplate.Model;
using Autofac.Extras.DynamicProxy;

namespace AspNetCore.StartUpTemplate.Services;
[Intercept(typeof(TransactionInterceptor))]
public class UserService:BaseServices<Users>,IUserService
{
    public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}