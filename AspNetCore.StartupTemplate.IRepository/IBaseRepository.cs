using SqlSugar;

namespace AspNetCore.StartUpTemplate.IRepository;

public interface IBaseRepository<T>:ISimpleClient<T> where T : class, new()
{
    
}