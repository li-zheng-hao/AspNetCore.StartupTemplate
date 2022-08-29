
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SqlSugar;

namespace {{NameSpacePrefix}}.IServices
{
	/// <summary>
    /// {{ModelDescription}} 服务工厂接口
    /// </summary>
    public interface I{{ModelClassName}}Services : IBaseServices<{{ModelClassName}}>
    {
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
        new Task<IPageList<{{ModelClassName}}>> QueryPageAsync(
            Expression<Func<{{ModelClassName}}, bool>> predicate,
            Expression<Func<{{ModelClassName}}, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);
        #endregion
    }
}
