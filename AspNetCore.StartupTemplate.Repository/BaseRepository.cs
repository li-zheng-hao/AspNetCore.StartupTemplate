using System.Linq.Expressions;
using AspNetCore.StartUpTemplate.IRepository;
using CoreCms.Net.Model.ViewModels.Basics;
using SqlSugar;

namespace AspNetCore.StartUpTemplate.Repository;

public class BaseRepository<T>:SimpleClient<T>,IBaseRepository<T> where T : class, new()
{
    private readonly IUnitOfWork _unitOfWork;
    private SqlSugarScope _dbBase=>_unitOfWork.GetDbClient();
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="unitOfWork"></param>
    public BaseRepository(IUnitOfWork unitOfWork) : base(unitOfWork.GetDbClient()) //注意这里要有默认值等于null
    {
        _unitOfWork = unitOfWork;
    }

    public Task<T> QueryByClauseAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public T QueryByClause(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false)
    {
        return blUseNoLock
            ? _unitOfWork.GetDbClient().Queryable<T>().OrderBy(orderByPredicate, orderByType).With(SqlWith.NoLock).First(predicate)
            : _unitOfWork.GetDbClient().Queryable<T>().OrderBy(orderByPredicate, orderByType).First(predicate);
    }

    public Task<T> QueryByClauseAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType,
        bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public int Insert(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> InsertAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public int Insert(T entity, Expression<Func<T, object>> insertColumns = null)
    {
        throw new NotImplementedException();
    }

    public Task<int> InsertAsync(T entity, Expression<Func<T, object>> insertColumns = null)
    {
        throw new NotImplementedException();
    }

    public bool InsertGuid(T entity, Expression<Func<T, object>> insertColumns = null)
    {
        throw new NotImplementedException();
    }

    public Task<bool> InsertGuidAsync(T entity, Expression<Func<T, object>> insertColumns = null)
    {
        throw new NotImplementedException();
    }

    public int Insert(List<T> entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> InsertAsync(List<T> entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> InsertCommandAsync(List<T> entity)
    {
        throw new NotImplementedException();
    }

    public bool Update(List<T> entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(List<T> entity)
    {
        throw new NotImplementedException();
    }

    public bool Update(T entity, string strWhere)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(T entity, string strWhere)
    {
        throw new NotImplementedException();
    }

    public bool Update(string strSql, SugarParameter[] parameters = null)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(string strSql, SugarParameter[] parameters = null)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(T entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "")
    {
        throw new NotImplementedException();
    }

    public bool Update(T entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "")
    {
        throw new NotImplementedException();
    }

    public bool Delete(IEnumerable<T> entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(IEnumerable<T> entity)
    {
        throw new NotImplementedException();
    }

    public bool DeleteByIds(int[] ids)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdsAsync(int[] ids)
    {
        throw new NotImplementedException();
    }

    public bool DeleteByIds(long[] ids)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdsAsync(long[] ids)
    {
        throw new NotImplementedException();
    }

    public bool DeleteByIds(Guid[] ids)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdsAsync(Guid[] ids)
    {
        throw new NotImplementedException();
    }

    public bool DeleteByIds(string[] ids)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdsAsync(string[] ids)
    {
        throw new NotImplementedException();
    }

    public bool DeleteByIds(List<int> ids)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdsAsync(List<int> ids)
    {
        throw new NotImplementedException();
    }

    public bool DeleteByIds(List<string> ids)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdsAsync(List<string> ids)
    {
        throw new NotImplementedException();
    }

    public bool DeleteByIds(List<Guid> ids)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdsAsync(List<Guid> ids)
    {
        throw new NotImplementedException();
    }

    public bool DeleteByIds(List<long> ids)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdsAsync(List<long> ids)
    {
        throw new NotImplementedException();
    }

    public bool Exists(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public int GetCount(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetCountAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public int GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field, bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field, bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public decimal GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> field, bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> field, bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public float GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> field, bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public Task<float> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> field, bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public IPageList<T> QueryPage(Expression<Func<T, bool>> predicate, string orderBy = "", int pageIndex = 1, int pageSize = 20,
        bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public Task<IPageList<T>> QueryPageAsync(Expression<Func<T, bool>> predicate, string orderBy = "", int pageIndex = 1, int pageSize = 20,
        bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public IPageList<T> QueryPage(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
        int pageSize = 20, bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public Task<IPageList<T>> QueryPageAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
        int pageSize = 20, bool blUseNoLock = false)
    {
        throw new NotImplementedException();
    }

    public List<TResult> QueryMuch<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression, Expression<Func<T1, T2, bool>> whereLambda = null,
        bool blUseNoLock = false) where T1 : class, new()
    {
        throw new NotImplementedException();
    }

    public Task<List<TResult>> QueryMuchAsync<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression,
        Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new()
    {
        throw new NotImplementedException();
    }

    public TResult QueryMuchFirst<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression,
        Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new()
    {
        throw new NotImplementedException();
    }

    public Task<TResult> QueryMuchFirstAsync<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression,
        Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new()
    {
        throw new NotImplementedException();
    }

    public List<TResult> QueryMuch<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, object[]>> joinExpression, Expression<Func<T1, T2, T3, TResult>> selectExpression,
        Expression<Func<T1, T2, T3, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new()
    {
        throw new NotImplementedException();
    }

    public Task<List<TResult>> QueryMuchAsync<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, object[]>> joinExpression, Expression<Func<T1, T2, T3, TResult>> selectExpression,
        Expression<Func<T1, T2, T3, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new()
    {
        throw new NotImplementedException();
    }

    public List<T> SqlQuery(string sql, List<SugarParameter> parameters)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> SqlQueryable(string sql)
    {
        throw new NotImplementedException();
    }

    public List<T> QueryByIDs(int[] lstIds, bool blUseNoLock = false)
    {
        return blUseNoLock
            ? _dbBase.Queryable<T>().In(lstIds).With(SqlWith.NoLock).ToList()
            : _dbBase.Queryable<T>().In(lstIds).ToList();
    }

    public async Task<List<T>> QueryByIDsAsync(int[] lstIds, bool blUseNoLock = false)
    {
        return  blUseNoLock
            ? await _dbBase.Queryable<T>().In(lstIds).With(SqlWith.NoLock).ToListAsync()
            : await _dbBase.Queryable<T>().In(lstIds).ToListAsync();
    }

    public List<T> QueryListByClause(string strWhere, string orderBy = "", bool blUseNoLock = false)
    {
        return blUseNoLock
            ? _dbBase.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).With(SqlWith.NoLock).ToList()
            : _dbBase.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList();
    }

    public async Task<List<T>> QueryListByClauseAsync(string strWhere, string orderBy = "", bool blUseNoLock = false)
    {
        return blUseNoLock
            ? await _dbBase.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).With(SqlWith.NoLock).ToListAsync()
            :await  _dbBase.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
    }

    public List<T> QueryListByClause(Expression<Func<T, bool>>? predicate, string orderBy = "", bool blUseNoLock = false)
    {
        return blUseNoLock
            ? _dbBase.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToList()
            : _dbBase.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                .WhereIF(predicate != null, predicate).ToList();
    }

    public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>>? predicate, string orderBy = "", bool blUseNoLock = false)
    {
        return blUseNoLock
            ? await _dbBase.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToListAsync()
            : await _dbBase.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                .WhereIF(predicate != null, predicate).ToListAsync();
    }

    public List<T> QueryListByClause(Expression<Func<T, bool>>? predicate, Expression<Func<T, object>>? orderByPredicate, OrderByType orderByType,
        bool blUseNoLock = false)
    {
        return blUseNoLock
            ? _dbBase.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToList()
            : _dbBase.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .WhereIF(predicate != null, predicate).ToList();
    }

    public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>>? predicate, Expression<Func<T, object>>? orderByPredicate, OrderByType orderByType,
        bool blUseNoLock = false)
    {
        return blUseNoLock
            ? await _dbBase.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToListAsync()
            : await _dbBase.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .WhereIF(predicate != null, predicate).ToListAsync();
    }

    public List<T> QueryListByClause(Expression<Func<T, bool>>? predicate, int take, Expression<Func<T, object>>? orderByPredicate, OrderByType orderByType,
        bool blUseNoLock = false)
    {
        return blUseNoLock
            ? _dbBase.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .WhereIF(predicate != null, predicate).Take(take).With(SqlWith.NoLock).ToList()
            : _dbBase.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .WhereIF(predicate != null, predicate).Take(take).ToList();
    }

    public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>>? predicate, int take, Expression<Func<T, object>>? orderByPredicate, OrderByType orderByType,
        bool blUseNoLock = false)
    {
        return blUseNoLock
            ? await _dbBase.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .WhereIF(predicate != null, predicate).Take(take).With(SqlWith.NoLock).ToListAsync()
            : await _dbBase.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                .WhereIF(predicate != null, predicate).Take(take).ToListAsync();
    }

    public List<T> QueryListByClause(Expression<Func<T, bool>> predicate, int take, string strOrderByFileds = "", bool blUseNoLock = false)
    {
        return blUseNoLock
            ? _dbBase.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                .Where(predicate).Take(take).With(SqlWith.NoLock).ToList()
            : _dbBase.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                .Where(predicate).Take(take).ToList();
    }

    public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, int take, string strOrderByFileds = "", bool blUseNoLock = false)
    {
        return blUseNoLock
            ? await _dbBase.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                .Where(predicate).Take(take).With(SqlWith.NoLock).ToListAsync()
            : await _dbBase.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                .Where(predicate).Take(take).ToListAsync();
    }

    public T QueryByClause(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
    {
        return blUseNoLock
            ? _dbBase.Queryable<T>().With(SqlWith.NoLock).First(predicate)
            : _dbBase.Queryable<T>().First(predicate);
    }
}