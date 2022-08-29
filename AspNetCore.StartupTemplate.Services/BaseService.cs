using System.Linq.Expressions;
using AspNetCore.StartUpTemplate.AOP;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.IService;
using CoreCms.Net.Model.ViewModels.Basics;
using SqlSugar;

namespace AspNetCore.StartUpTemplate.Services;

public class BaseServices<T> :IBaseService<T> where T : class, new()
{
    public IBaseRepository<T> BaseRepo = null!; //通过在子类的构造函数中注入，这里是基类，不用构造函数
    private readonly IUnitOfWork _unitOfWork;
    public BaseServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public T QueryById(object pkValue)
    {
        return BaseRepo.GetById(pkValue);
    }

    public async Task<T> QueryByIdAsync(object objId)
    {
        return await BaseRepo.GetByIdAsync(objId);
    }


    public List<T> QueryByIDs(int[] lstIds)
    {
        return BaseRepo.QueryByIDs(lstIds);
    }

    public async Task<List<T>> QueryByIDsAsync(int[] lstIds)
    {
        return await BaseRepo.QueryByIDsAsync(lstIds);

    }
    [UseTransaction(typeof(Exception))]
    public List<T> Query()
    {
        throw new ArgumentException("xxx");
        return BaseRepo.GetList();

    }

    public async Task<List<T>> QueryAsync()
    {
        return await BaseRepo.GetListAsync();
    }

    public List<T> QueryListByClause(string strWhere, string orderBy = "")
    {
        return BaseRepo.QueryListByClause(strWhere, orderBy);
    }

    public async Task<List<T>> QueryListByClauseAsync(string strWhere, string orderBy = "")
    {
        return await BaseRepo.QueryListByClauseAsync(strWhere, orderBy);

    }

    public List<T> QueryListByClause(Expression<Func<T, bool>>? predicate, string orderBy = "")
    {
        return  BaseRepo.QueryListByClause(predicate, orderBy);

    }

    public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>>? predicate, string orderBy = "")
    {
        return  await BaseRepo.QueryListByClauseAsync(predicate, orderBy);
    }

    public List<T> QueryListByClause(Expression<Func<T, bool>>? predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType)
    {
        return  BaseRepo.QueryListByClause(predicate, orderByPredicate,orderByType);

    }

    public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>>? predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType)
    {
        return  await BaseRepo.QueryListByClauseAsync(predicate, orderByPredicate,orderByType);

    }

    public List<T> QueryListByClause(Expression<Func<T, bool>>? predicate, int take, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType)
    {
        return  BaseRepo.QueryListByClause(predicate,take, orderByPredicate,orderByType);

    }

    public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>>? predicate, int take, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType)
    {
        return  await BaseRepo.QueryListByClauseAsync(predicate,take, orderByPredicate,orderByType);

    }

    public List<T> QueryListByClause(Expression<Func<T, bool>>? predicate, int take, string strOrderByFileds = "")
    {
        return  BaseRepo.QueryListByClause(predicate,take, strOrderByFileds);

    }

    public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>>? predicate, int take, string strOrderByFileds = "")
    {
        return  await BaseRepo.QueryListByClauseAsync(predicate,take, strOrderByFileds);
    }

    public T QueryByClause(Expression<Func<T, bool>>? predicate)
    {
        return  BaseRepo.QueryByClause(predicate);

    }

    public async Task<T> QueryByClauseAsync(Expression<Func<T, bool>>? predicate)
    {
        return  await BaseRepo.QueryByClauseAsync(predicate);

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
        var entity = BaseRepo.QueryByClause(predicate, orderByPredicate, orderByType);
        return entity;
    }

    public async Task<T> QueryByClauseAsync(Expression<Func<T, bool>>? predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType)
    { 
        var entity =await  BaseRepo.QueryByClauseAsync(predicate, orderByPredicate, orderByType);
        return entity;
    }

    public int Insert(T entity)
    {
        return BaseRepo.InsertReturnIdentity(entity);
    }

    public async Task<int> InsertAsync(T entity)
    {
        return await BaseRepo.InsertReturnIdentityAsync(entity);

    }

    public int Insert(T entity, Expression<Func<T, object>> insertColumns = null)
    {
        return  BaseRepo.Insert(entity,insertColumns);

    }

    public async Task<int> InsertAsync(T entity, Expression<Func<T, object>> insertColumns = null)
    {
        return await BaseRepo.InsertAsync(entity, insertColumns);

    }

    public bool InsertGuid(T entity, Expression<Func<T, object>> insertColumns = null)
    {
        return BaseRepo.InsertGuid(entity, insertColumns);
    }

    public async Task<bool> InsertGuidAsync(T entity, Expression<Func<T, object>> insertColumns = null)
    {
        return await BaseRepo.InsertGuidAsync(entity, insertColumns);
    }

    public int Insert(List<T> entity)
    {
        return BaseRepo.Insert(entity);
    }

    public Task<int> InsertAsync(List<T> entity)
    {
        return BaseRepo.InsertAsync(entity);

    }

    public bool Update(List<T> entity)
    {
        return BaseRepo.Update(entity);


    }

    public async Task<bool> UpdateAsync(List<T> entity)
    {
        return await BaseRepo.UpdateAsync(entity);

    }

    public bool Update(T entity)
    {
        return BaseRepo.Update(entity);

    }

    public async Task<bool> UpdateAsync(T entity)
    {
        return await BaseRepo.UpdateAsync(entity);
    }

    public bool Update(T entity, string strWhere)
    {
        return BaseRepo.Update(entity, strWhere);

    }

    public async Task<bool> UpdateAsync(T entity, string strWhere)
    {
        return await BaseRepo.UpdateAsync(entity, strWhere);
    }

    public bool Update(string strSql, SugarParameter[] parameters = null)
    {
        return  BaseRepo.Update(strSql, parameters);
    }

    public async Task<bool> UpdateAsync(string strSql, SugarParameter[] parameters = null)
    {
        return await BaseRepo.UpdateAsync(strSql, parameters);

    }

    public bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>>? where)
    {
        return BaseRepo.Update(columns, where);
    }

    public async Task<bool> UpdateAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>>? where)
    {
        return await BaseRepo.UpdateAsync(columns, where);
    }

    public async Task<bool> UpdateAsync(T entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "")
    {
        return await BaseRepo.UpdateAsync(entity, lstColumns, lstIgnoreColumns, strWhere);
    }

    public bool Update(T entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "")
    {
        return BaseRepo.Update(entity, lstColumns, lstIgnoreColumns, strWhere);

    }

    public bool Delete(T entity)
    {
        return BaseRepo.Delete(entity);

    }

    public async Task<bool> DeleteAsync(T entity)
    {
        return await BaseRepo.DeleteAsync(entity);

    }

    public bool Delete(IEnumerable<T> entity)
    {
        return  BaseRepo.Delete(entity);

    }

    public async Task<bool> DeleteAsync(IEnumerable<T> entity)
    {
        return  await BaseRepo.DeleteAsync(entity);

    }

    public bool Delete(Expression<Func<T, bool>>? where)
    {
        return  BaseRepo.Delete(where);

    }

    public async Task<bool> DeleteAsync(Expression<Func<T, bool>>? where)
    {
        return await  BaseRepo.DeleteAsync(where);
    }

    public bool DeleteById(object id)
    {
        return   BaseRepo.DeleteById(id);
    }

    public async Task<bool> DeleteByIdAsync(object id)
    {
        return   await BaseRepo.DeleteByIdAsync(id);
    }

    public bool DeleteByIds(int[] ids)
    {
        return  BaseRepo.DeleteByIds(ids);
    }

    public async Task<bool> DeleteByIdsAsync(int[] ids)
    {
        return await BaseRepo.DeleteByIdsAsync(ids);

    }

    
    public bool DeleteByIds(Guid[] ids)
    {
        return BaseRepo.DeleteByIds(ids);

    }

    public async Task<bool> DeleteByIdsAsync(Guid[] ids)
    {
        return await BaseRepo.DeleteByIdsAsync(ids);

    }

    public bool DeleteByIds(string[] ids)
    {
        return BaseRepo.DeleteByIds(ids);

    }

    public async Task<bool> DeleteByIdsAsync(string[] ids)
    {
        return await BaseRepo.DeleteByIdsAsync(ids);

    }

    public bool DeleteByIds(List<int> ids)
    {
        return BaseRepo.DeleteByIds(ids);

    }

    public async Task<bool> DeleteByIdsAsync(List<int> ids)
    {
        return await BaseRepo.DeleteByIdsAsync(ids);

    }

    public bool DeleteByIds(List<string> ids)
    {
        return BaseRepo.DeleteByIds(ids);

    }

    public async Task<bool> DeleteByIdsAsync(List<string> ids)
    {
        return await BaseRepo.DeleteByIdsAsync(ids);

    }

    public bool DeleteByIds(List<Guid> ids)
    {
        return BaseRepo.DeleteByIds(ids);

    }

    public async Task<bool> DeleteByIdsAsync(List<Guid> ids)
    {
        return await BaseRepo.DeleteByIdsAsync(ids);

    }


    public bool Exists(Expression<Func<T, bool>>? predicate)
    {
        return BaseRepo.Exists(predicate);

    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>>? predicate)
    {
        return await BaseRepo.ExistsAsync(predicate);

    }

    public int GetCount(Expression<Func<T, bool>>? predicate)
    {
        return BaseRepo.GetCount(predicate);

    }

    public async Task<int> GetCountAsync(Expression<Func<T, bool>>? predicate)
    {
        return await BaseRepo.GetCountAsync(predicate);

    }

    public int GetSum(Expression<Func<T, bool>>? predicate, Expression<Func<T, int>> field)
    {
        return BaseRepo.GetSum(predicate, field);

    }

    public async Task<int> GetSumAsync(Expression<Func<T, bool>>? predicate, Expression<Func<T, int>> field, bool blUseNoLock = false)
    {
        return await BaseRepo.GetSumAsync(predicate, field);

    }

    public decimal GetSum(Expression<Func<T, bool>>? predicate, Expression<Func<T, decimal>> field, bool blUseNoLock = false)
    {
        return BaseRepo.GetSum(predicate, field);

    }

    public async Task<decimal> GetSumAsync(Expression<Func<T, bool>>? predicate, Expression<Func<T, decimal>> field, bool blUseNoLock = false)
    {
        return await BaseRepo.GetSumAsync(predicate, field);

    }

    public float GetSum(Expression<Func<T, bool>>? predicate, Expression<Func<T, float>> field)
    {
        return  BaseRepo.GetSum(predicate, field);

    }

    public async Task<float> GetSumAsync(Expression<Func<T, bool>>? predicate, Expression<Func<T, float>> field)
    {
        return await BaseRepo.GetSumAsync(predicate, field);


    }

    public IPageList<T> QueryPage(Expression<Func<T, bool>>? predicate, string orderBy = "", int pageIndex = 1, int pageSize = 20)
    {
        return  BaseRepo.QueryPage(predicate, orderBy, pageIndex, pageSize);

    }

    public async Task<IPageList<T>> QueryPageAsync(Expression<Func<T, bool>>? predicate, string orderBy = "", int pageIndex = 1, int pageSize = 20)
    {
        return await  BaseRepo.QueryPageAsync(predicate, orderBy, pageIndex, pageSize);
    }

    public IPageList<T> QueryPage(Expression<Func<T, bool>>? predicate, Expression<Func<T, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
        int pageSize = 20)
    {
        return  BaseRepo.QueryPage(predicate, orderByExpression, orderByType, pageIndex, pageSize);
    }

    public async Task<IPageList<T>> QueryPageAsync(Expression<Func<T, bool>>? predicate, Expression<Func<T, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
        int pageSize = 20)
    {
        return  await BaseRepo.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize);

    }

    public List<TResult> QueryMuch<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression, Expression<Func<T1, T2, bool>> whereLambda = null) where T1 : class, new()
    {
        return BaseRepo.QueryMuch(joinExpression, selectExpression, whereLambda);

    }

    public async Task<List<TResult>> QueryMuchAsync<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression,
        Expression<Func<T1, T2, bool>> whereLambda = null) where T1 : class, new()
    {
        return await BaseRepo.QueryMuchAsync(joinExpression, selectExpression, whereLambda);

    }

    public TResult QueryMuchFirst<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression,
        Expression<Func<T1, T2, bool>> whereLambda = null) where T1 : class, new()
    {
        return BaseRepo.QueryMuchFirst(joinExpression, selectExpression, whereLambda);

    }

    public async Task<TResult> QueryMuchFirstAsync<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression,
        Expression<Func<T1, T2, bool>> whereLambda = null) where T1 : class, new()
    {
        return await BaseRepo.QueryMuchFirstAsync(joinExpression, selectExpression, whereLambda);

    }

    public List<TResult> QueryMuch<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, object[]>> joinExpression, Expression<Func<T1, T2, T3, TResult>> selectExpression,
        Expression<Func<T1, T2, T3, bool>> whereLambda = null) where T1 : class, new()
    {
        return BaseRepo.QueryMuch(joinExpression, selectExpression, whereLambda);

    }

    public async Task<List<TResult>> QueryMuchAsync<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, object[]>> joinExpression, Expression<Func<T1, T2, T3, TResult>> selectExpression,
        Expression<Func<T1, T2, T3, bool>> whereLambda = null) where T1 : class, new()
    {
        return await BaseRepo.QueryMuchAsync(joinExpression, selectExpression, whereLambda);

    }

    public List<T> SqlQuery(string sql, List<SugarParameter> parameters)
    {
        return BaseRepo.SqlQuery(sql, parameters);

    }

    public async Task<List<T>> SqlQueryable(string sql)
    {
        return await BaseRepo.SqlQueryable(sql);

    }
}