using System.Collections.Generic;
using System.Linq;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.Model;

namespace AspNetCore.StartUpTemplate.Repository
{
    /// <summary>
    /// 仓储模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UserRepository : CurBaseRepository<Users,long>, IUserRepository
    {
        public UserRepository(IFreeSql fsql) : base(fsql)
        {
        
        }
    }
}