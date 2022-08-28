using AspNetCore.StartUpTemplate.IRepository;
using SqlSugar;

namespace AspNetCore.StartUpTemplate.Repository;

public class BaseRepository<T>:SimpleClient<T>,IBaseRepository<T> where T : class, new()
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="unitOfWork"></param>
    public BaseRepository(IUnitOfWork unitOfWork) : base(unitOfWork.GetDbClient()) //注意这里要有默认值等于null
    {
        _unitOfWork = unitOfWork;
    }
}