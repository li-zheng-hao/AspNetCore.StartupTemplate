using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SqlSugar;
namespace {{NameSpacePrefix}}.Repository;
/// <summary>
/// {{ModelDescription}} 接口实现
/// </summary>
public class {{ModelClassName}}Repository : BaseRepository<{{ModelClassName}}>, I{{ModelClassName}}Repository
{
    public {{ModelClassName}}Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    #region 重写根据条件查询分页数据
    /// <summary>
    ///     重写根据条件查询分页数据
    /// </summary>
    /// <param name="predicate">判断集合</param>
    /// <param name="orderByType">排序方式</param>
    /// <param name="pageIndex">当前页面索引</param>
    /// <param name="pageSize">分布大小</param>
    /// <param name="orderByExpression"></param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns></returns>
    public new async Task<IPageList<{{ModelClassName}}>> QueryPageAsync(Expression<Func<{{ModelClassName}}, bool>> predicate,
        Expression<Func<{{ModelClassName}}, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
        int pageSize = 20, bool blUseNoLock = false)
    {
        RefAsync<int> totalCount = 0;
        List<{{ModelClassName}}> page;
        if (blUseNoLock)
        {
            page = await DbClient.Queryable<{{ModelClassName}}>()
            .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
            .WhereIF(predicate != null, predicate).Select(p => new {{ModelClassName}}
            {
                  {% for field in ModelFields %}{{field.DbColumnName}} = p.{{field.DbColumnName}},
            {% endfor %}
            }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
        }
        else
        {
            page = await DbClient.Queryable<{{ModelClassName}}>()
            .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
            .WhereIF(predicate != null, predicate).Select(p => new {{ModelClassName}}
            {
                  {% for field in ModelFields %}{{field.DbColumnName}} = p.{{field.DbColumnName}},
            {% endfor %}
            }).ToPageListAsync(pageIndex, pageSize, totalCount);
        }
        var list = new PageList<{{ModelClassName}}>(page, pageIndex, pageSize, totalCount);
        return list;
    }

    #endregion

}
