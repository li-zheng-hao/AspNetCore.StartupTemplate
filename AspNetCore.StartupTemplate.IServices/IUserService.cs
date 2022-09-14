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

 List<Users> QueryAll();

 Users Query(string key);

 List<Users> PageQuery(int number, int size);
 void JoinQuery();

 public  Task CapWithFreeSqlTrans();
 public Task CapWithFreeSqlTransRollBack();
}