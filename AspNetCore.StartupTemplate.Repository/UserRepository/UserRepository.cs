using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.Model;

namespace AspNetCore.StartUpTemplate.Repository
{
    /// <summary>
    /// 仓储模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UserRepository : BaseRepository<Users>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            Console.WriteLine(unitOfWork.GetDbClient().ContextID.ToString()+"  repository层");
        }
    }
}