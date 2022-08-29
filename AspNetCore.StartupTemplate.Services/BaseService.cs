using System.Linq.Expressions;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.IService;
using CoreCms.Net.Model.ViewModels.Basics;
using SqlSugar;

namespace AspNetCore.StartUpTemplate.Services;

public class BaseServices<T> :IBaseService<T> where T : class, new()
{
    private IBaseRepository<T> _baseRepo = null!; //通过在子类的构造函数中注入，这里是基类，不用构造函数
    private readonly IUnitOfWork _unitOfWork;
    public BaseServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public T QueryById(object pkValue)
    {
        return _baseRepo.GetById(pkValue);
    }

    public async Task<T> QueryByIdAsync(object objId)
    {
        return await _baseRepo.GetByIdAsync(objId);
    }


    public List<T> QueryByIDs(int[] lstIds)
    {
        return _baseRepo.QueryByIDs(lstIds);
    }

    public async Task<List<T>> QueryByIDsAsync(int[] lstIds)
    {
        return await _baseRepo.QueryByIDsAsync(lstIds);

    }

    public List<T> Query()
    {
        return _baseRepo.GetList();

    }

    public async Task<List<T>> QueryAsync()
    {
        return await _baseRepo.GetListAsync();
    }

    public List<T> QueryListByClause(string strWhere, string orderBy = "")
    {
        return _baseRepo.QueryListByClause(strWhere, orderBy);
    }

    public async Task<List<T>> QueryListByClauseAsync(string strWhere, string orderBy = "")
    {
        return await _baseRepo.QueryListByClauseAsync(strWhere, orderBy);

    }

    public List<T> QueryListByClause(Expression<Func<T, bool>>? predicate, string orderBy = "")
    {
        return  _baseRepo.QueryListByClause(predicate, orderBy);

    }

    public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>>? predicate, string orderBy = "")
    {
        return  await _baseRepo.QueryListByClauseAsync(predicate, orderBy);
    }

    public List<T> QueryListByClause(Expression<Func<T, bool>>? predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType)
    {
        return  _baseRepo.QueryListByClause(predicate, orderByPredicate,orderByType);

    }

    public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>>? predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType)
    {
        return  await _baseRepo.QueryListByClauseAsync(predicate, orderByPredicate,orderByType);

    }

    public List<T> QueryListByClause(Expression<Func<T, bool>>? predicate, int take, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType)
    {
        return  _baseRepo.QueryListByClause(predicate,take, orderByPredicate,orderByType);

    }

    public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>>? predicate, int take, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType)
    {
        return  await _baseRepo.QueryListByClauseAsync(predicate,take, orderByPredicate,orderByType);

    }

    public List<T> QueryListByClause(Expression<Func<T, bool>>? predicate, int take, string strOrderByFileds = "")
    {
        return  _baseRepo.QueryListByClause(predicate,take, strOrderByFileds);

    }

    public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>>? predicate, int take, string strOrderByFileds = "")
    {
        return  await _baseRepo.QueryListByClauseAsync(predicate,take, strOrderByFileds);
    }

    public T QueryByClause(Expression<Func<T, bool>>? predicate)
    {
        return  _baseRepo.QueryByClause(predicate);

    }

    public async Task<T> QueryByClauseAsync(Expression<Func<T, bool>>? predicate)
    {
        return  await _baseRepo.QueryByClauseAsync(predicate);

    }


    /// <summary>
    ///     根据条件查询数据
    /// </summary>
    /// <param name="predicate">条件表达式树</param>
    /// <param name="orderByPredicate">排序字段</param>
    /// <param name="orderByType">排序顺序</param>
    /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
    /// <returns></returns>
    public T QueryByClause(Expression<Func<T, bool>>? predicate, Expression<Func<T, object>> orderByPredicate,
        OrderByType orderByType)
    {
        var entity = _baseRepo.QueryByClause(predicate, orderByPredicate, orderByType);
        return entity;
    }

    public async Task<T> QueryByClauseAsync(Expression<Func<T, bool>>? predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType)
    { 
        var entity =await  _baseRepo.QueryByClauseAsync(predicate, orderByPredicate, orderByType);
        return entity;
    }

    public int Insert(T entity)
    {
        return _baseRepo.InsertReturnIdentity(entity);
    }

    public async Task<int> InsertAsync(T entity)
    {
        return await _baseRepo.InsertReturnIdentityAsync(entity);

    }

    public int Insert(T entity, Expression<Func<T, object>> insertColumns = null)
    {
        return  _baseRepo.Insert(entity,insertColumns);

    }

    public async Task<int> InsertAsync(T entity, Expression<Func<T, object>> insertColumns = null)
    {
        return await _baseRepo.InsertAsync(entity, insertColumns);

    }

    public bool InsertGuid(T entity, Expression<Func<T, object>> insertColumns = null)
    {
        return _baseRepo.InsertGuid(entity, insertColumns);
    }

    public async Task<bool> InsertGuidAsync(T entity, Expression<Func<T, object>> insertColumns = null)
    {
        return await _baseRepo.InsertGuidAsync(entity, insertColumns);
    }

    public int Insert(List<T> entity)
    {
        return _baseRepo.Insert(entity);
    }

    public Task<int> InsertAsync(List<T> entity)
    {
        return _baseRepo.InsertAsync(entity);

    }

    public bool Update(List<T> entity)
    {
        return _baseRepo.Update(entity);


    }

    public async Task<bool> UpdateAsync(List<T> entity)
    {
        return await _baseRepo.UpdateAsync(entity);

    }

    public bool Update(T entity)
    {
        return _baseRepo.Update(entity);

    }

    public async Task<bool> UpdateAsync(T entity)
    {
        return await _baseRepo.UpdateAsync(entity);
    }

    public bool Update(T entity, string strWhere)
    {
        return _baseRepo.Update(entity, strWhere);

    }

    public async Task<bool> UpdateAsync(T entity, string strWhere)
    {
        return await _baseRepo.UpdateAsync(entity, strWhere);
    }

    public bool Update(string strSql, SugarParameter[] parameters = null)
    {
        return  _baseRepo.Update(strSql, parameters);
    }

    public async Task<bool> UpdateAsync(string strSql, SugarParameter[] parameters = null)
    {
        return await _baseRepo.UpdateAsync(strSql, parameters);

    }

    public bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>>? where)
    {
        return _baseRepo.Update(columns, where);
    }

    public async Task<bool> UpdateAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>>? where)
    {
        return await _baseRepo.UpdateAsync(columns, where);
    }

    public async Task<bool> UpdateAsync(T entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "")
    {
        return await _baseRepo.UpdateAsync(entity, lstColumns, lstIgnoreColumns, strWhere);
    }

    public bool Update(T entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "")
    {
        return _baseRepo.Update(entity, lstColumns, lstIgnoreColumns, strWhere);

    }

    public bool Delete(T entity)
    {
        return _baseRepo.Delete(entity);

    }

    public async Task<bool> DeleteAsync(T entity)
    {
        return await _baseRepo.DeleteAsync(entity);

    }

    public bool Delete(IEnumerable<T> entity)
    {
        return  _baseRepo.Delete(entity);

    }

    public async Task<bool> DeleteAsync(IEnumerable<T> entity)
    {
        return  await _baseRepo.DeleteAsync(entity);

    }

    public bool Delete(Expression<Func<T, bool>>? where)
    {
        return  _baseRepo.Delete(where);

    }

    public async Task<bool> DeleteAsync(Expression<Func<T, bool>>? where)
    {
        return await  _baseRepo.DeleteAsync(where);
    }

    public bool DeleteById(object id)
    {
        return   _baseRepo.DeleteById(id);
    }

    public async Task<bool> DeleteByIdAsync(object id)
    {
        return   await _baseRepo.DeleteByIdAsync(id);
    }

    public bool DeleteByIds(int[] ids)
    {
        return  _baseRepo.DeleteByIds(ids);
    }

    public async Task<bool> DeleteByIdsAsync(int[] ids)
    {
        return await _baseRepo.DeleteByIdsAsync(ids);

    }

    
    public bool DeleteByIds(Guid[] ids)
    {
        return _baseRepo.DeleteByIds(ids);

    }

    public async Task<bool> DeleteByIdsAsync(Guid[] ids)
    {
        return await _baseRepo.DeleteByIdsAsync(ids);

    }

    public bool DeleteByIds(string[] ids)
    {
        return _baseRepo.DeleteByIds(ids);

    }

    public async Task<bool> DeleteByIdsAsync(string[] ids)
    {
        return await _baseRepo.DeleteByIdsAsync(ids);

    }

    public bool DeleteByIds(List<int> ids)
    {
        return _baseRepo.DeleteByIds(ids);

    }

    public async Task<bool> DeleteByIdsAsync(List<int> ids)
    {
        return await _baseRepo.DeleteByIdsAsync(ids);

    }

    public bool DeleteByIds(List<string> ids)
    {
        return _baseRepo.DeleteByIds(ids);

    }

    public async Task<bool> DeleteByIdsAsync(List<string> ids)
    {
        return await _baseRepo.DeleteByIdsAsync(ids);

    }

    public bool DeleteByIds(List<Guid> ids)
    {
        return _baseRepo.DeleteByIds(ids);

    }

    public async Task<bool> DeleteByIdsAsync(List<Guid> ids)
    {
        return await _baseRepo.DeleteByIdsAsync(ids);

    }


    public bool Exists(Expression<Func<T, bool>>? predicate)
    {
        return _baseRepo.Exists(predicate);

    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>>? predicate)
    {
        return await _baseRepo.ExistsAsync(predicate);

    }

    public int GetCount(Expression<Func<T, bool>>? predicate)
    {
        return _baseRepo.GetCount(predicate);

    }

    public async Task<int> GetCountAsync(Expression<Func<T, bool>>? predicate)
    {
        return await _baseRepo.GetCountAsync(predicate);

    }

    public int GetSum(Expression<Func<T, bool>>? predicate, Expression<Func<T, int>> field)
    {
        return _baseRepo.GetSum(predicate, field);

    }

    public async Task<int> GetSumAsync(Expression<Func<T, bool>>? predicate, Expression<Func<T, int>> field, bool blUseNoLock = false)
    {
        return await _baseRepo.GetSumAsync(predicate, field);

    }

    public decimal GetSum(Expression<Func<T, bool>>? predicate, Expression<Func<T, decimal>> field, bool blUseNoLock = false)
    {
        return _baseRepo.GetSum(predicate, field);

    }

    public async Task<decimal> GetSumAsync(Expression<Func<T, bool>>? predicate, Expression<Func<T, decimal>> field, bool blUseNoLock = false)
    {
        return await _baseRepo.GetSumAsync(predicate, field);

    }

    public float GetSum(Expression<Func<T, bool>>? predicate, Expression<Func<T, float>> field)
    {
        return  _baseRepo.GetSum(predicate, field);

    }

    public async Task<float> GetSumAsync(Expression<Func<T, bool>>? predicate, Expression<Func<T, float>> field)
    {
        return await _baseRepo.GetSumAsync(predicate, field);


    }

    public IPageList<T> QueryPage(Expression<Func<T, bool>>? predicate, string orderBy = "", int pageIndex = 1, int pageSize = 20)
    {
        return  _baseRepo.QueryPage(predicate, orderBy, pageIndex, pageSize);

    }

    public async Task<IPageList<T>> QueryPageAsync(Expression<Func<T, bool>>? predicate, string orderBy = "", int pageIndex = 1, int pageSize = 20)
    {
        return await  _baseRepo.QueryPageAsync(predicate, orderBy, pageIndex, pageSize);
    }

    public IPageList<T> QueryPage(Expression<Func<T, bool>>? predicate, Expression<Func<T, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
        int pageSize = 20)
    {
        return  _baseRepo.QueryPage(predicate, orderByExpression, orderByType, pageIndex, pageSize);
    }

    public async Task<IPageList<T>> QueryPageAsync(Expression<Func<T, bool>>? predicate, Expression<Func<T, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
        int pageSize = 20)
    {
        return  await _baseRepo.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize);

    }

    public List<TResult> QueryMuch<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression, Expression<Func<T1, T2, bool>> whereLambda = null) where T1 : class, new()
    {
        return _baseRepo.QueryMuch(joinExpression, selectExpression, whereLambda);

    }

    public async Task<List<TResult>> QueryMuchAsync<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression,
        Expression<Func<T1, T2, bool>> whereLambda = null) where T1 : class, new()
    {
        return await _baseRepo.QueryMuchAsync(joinExpression, selectExpression, whereLambda);

    }

    public TResult QueryMuchFirst<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression,
        Expression<Func<T1, T2, bool>> whereLambda = null) where T1 : class, new()
    {
        return _baseRepo.QueryMuchFirst(joinExpression, selectExpression, whereLambda);

    }

    public async Task<TResult> QueryMuchFirstAsync<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression,
        Expression<Func<T1, T2, bool>> whereLambda = null) where T1 : class, new()
    {
        return await _baseRepo.QueryMuchFirstAsync(joinExpression, selectExpression, whereLambda);

    }

    public List<TResult> QueryMuch<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, object[]>> joinExpression, Expression<Func<T1, T2, T3, TResult>> selectExpression,
        Expression<Func<T1, T2, T3, bool>> whereLambda = null) where T1 : class, new()
    {
        return _baseRepo.QueryMuch(joinExpression, selectExpression, whereLambda);

    }

    public async Task<List<TResult>> QueryMuchAsync<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, object[]>> joinExpression, Expression<Func<T1, T2, T3, TResult>> selectExpression,
        Expression<Func<T1, T2, T3, bool>> whereLambda = null) where T1 : class, new()
    {
        return await _baseRepo.QueryMuchAsync(joinExpression, selectExpression, whereLambda);

    }

    public List<T> SqlQuery(string sql, List<SugarParameter> parameters)
    {
        return _baseRepo.SqlQuery(sql, parameters);

    }

    public async Task<List<T>> SqlQueryable(string sql)
    {
        return await _baseRepo.SqlQueryable(sql);

    }
}