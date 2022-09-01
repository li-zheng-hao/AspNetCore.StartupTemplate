using System.Linq.Expressions;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.IRepository;
using CoreCms.Net.Model.ViewModels.Basics;
using FreeSql;

namespace AspNetCore.StartUpTemplate.Repository;

public class CurBaseRepository<TSource,TKey>:BaseRepository<TSource, TKey> where TSource : class
{
    public CurBaseRepository(IFreeSql fsql) : base(fsql, null, null) {}
}