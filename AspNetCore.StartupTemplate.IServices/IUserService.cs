using AspNetCore.StartUpTemplate.Model;
using Autofac.Extras.DynamicProxy;

namespace AspNetCore.StartUpTemplate.IService;
public interface IUserService
{
 void FuncA();

 void ChangeMoney(int userid,int number);
 void ChangeMoneyError(int userid,int number);
 void InsertUserBatch();

 void UpdateBatch();

 void QueryAll();

 void PageQuery(int number, int size);
 void JoinQuery();
}