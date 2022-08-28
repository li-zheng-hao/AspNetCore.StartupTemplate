using SqlSugar;

namespace AspNetCore.StartUpTemplate.IRepository;

public interface IUnitOfWork
{
    SqlSugarScope GetDbClient();

    void BeginTran();

    void CommitTran();
    void RollbackTran();
}