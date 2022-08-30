using SqlSugar;

namespace AspNetCore.StartUpTemplate.Core;

public interface IUnitOfWork:IDisposable
{
    ISqlSugarClient GetDbClient();
    void SetDbClient(ISqlSugarClient client);
    void BeginTran();

    void CommitTran();
    void RollbackTran();

    bool IsUsingTransaction();
    void Reset();
    
    bool IsNestedTrans();

    void RollbackToSavepoint(string savepoint);
    
    void SetSavepoint(string savepointName);
}