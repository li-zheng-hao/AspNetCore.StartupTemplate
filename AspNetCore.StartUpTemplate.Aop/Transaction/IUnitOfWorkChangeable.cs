using AspNetCore.StartUpTemplate.Core;
using SqlSugar;

namespace AspNetCore.StartUpTemplate.AOP;

public interface IUnitOfWorkChangeable
{
    void SetUnitOfWork(IUnitOfWork unitOfWork);

    IUnitOfWork GetUnitOfWork();

   
    
}