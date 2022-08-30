using AspNetCore.StartUpTemplate.Core;
using SqlSugar;

namespace AspNetCore.StartUpTemplate.AOP;

public interface IUnitOfWorkChangeable
{
    void ResetDb(ISqlSugarClient sugarClient);

    IUnitOfWork GetUnitOfWork();

   
    
}