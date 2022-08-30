using AspNetCore.StartUpTemplate.Model;
using Autofac.Extras.DynamicProxy;

namespace AspNetCore.StartUpTemplate.IService;
public interface IUserService: IBaseService<Users> 
{
   
}