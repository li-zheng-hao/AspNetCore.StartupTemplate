using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.Utility;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Bcpg.Sig;

namespace AspNetCore.StartUpTemplate.AOP;

/// <summary>
/// 事务处理AOP
/// </summary>
public class TransactionInterceptor : IInterceptor
{
    private readonly ILogger<TransactionInterceptor> _logger;
    private UseTransactionAttribute _useTransactionAttribute { get; set; }
    /// <summary>
    /// 通过属性注入
    /// </summary>
    private IUnitOfWork _unitOfWork { get; set; }
    public TransactionInterceptor(ILogger<TransactionInterceptor> logger)
    {
        _logger = logger;
        
    }
   
    public void Intercept(IInvocation invocation)
    {
#if DEBUG  
        Console.WriteLine("你正在调用方法 \"{0}\"  参数是 {1}... ",
            invocation.Method.Name,              
            string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));
#endif
        _unitOfWork= GetCurUnitOfWork(invocation);

        // 加在interface上
        var attributeOnInterface = invocation.Method.GetAttribute<UseTransactionAttribute>();
        // 加在了实现类上
        var attributeOnImpl = invocation.MethodInvocationTarget.GetAttribute<UseTransactionAttribute>();
        if (attributeOnInterface != null)
        {
            _useTransactionAttribute = attributeOnInterface;
        }else if (attributeOnImpl != null)
        {
            _useTransactionAttribute = attributeOnImpl;
        }
        CheckAndBeginTrans(invocation);
        try
        {
            invocation.Proceed();
        }
        catch (Exception e)
        {
            RollbackTransWhenException(invocation,e);
            _logger.LogError("AOP层捕获到异常",e);
            throw;
        }
        // 方法结束提交
        if(_unitOfWork.IsUsingTransaction()&&_useTransactionAttribute.Propagation!=Propagation.Nested)
            _unitOfWork.CommitTran();
#if DEBUG  
        Console.WriteLine("方法{0}执行完毕，返回结果：{1}",invocation.Method.Name, invocation.ReturnValue);
#endif
    }
    /// <summary>
    /// 检查是否需要开启事务及开启事务
    /// </summary>
    /// <param name="invocation"></param>
    private void CheckAndBeginTrans(IInvocation invocation)
    {
        if (_useTransactionAttribute == null) return;
        switch (_useTransactionAttribute.Propagation)
        {
            case Propagation.Required:
                if (_unitOfWork.IsUsingTransaction()) break;
                _unitOfWork.BeginTran();
                break;
            case Propagation.RequireNew:
                var targetObj=invocation.InvocationTarget as IUnitOfWorkChangeable;
                if (targetObj == null)
                    throw new Exception($"标记了{Propagation.RequireNew}的方法所在类必须实现了IUnitOfWorkChangeable接口");
                targetObj.ResetDb(SqlSugarConfig.GetSugarClient());
                var uow=targetObj.GetUnitOfWork();
                uow.BeginTran();
                break;
            case Propagation.Supports:
                // 无需处理
            case Propagation.Never:
                // 有事务异常
                if (_unitOfWork.IsUsingTransaction())
                    throw new Exception(nameof(Propagation.Never) + "下不允许开启事务");
                break;
            case Propagation.Nested:
                var targetObjNested=invocation.InvocationTarget as IUnitOfWorkChangeable;
                if (targetObjNested.GetUnitOfWork().IsUsingTransaction() == false)
                {
                    _unitOfWork.BeginTran();
                }
                else
                {
                    targetObjNested.GetUnitOfWork().GetDbClient().Ado.ExecuteCommand("savepoint nested");
                    targetObjNested.GetUnitOfWork().SetSavepoint("nested");
                }
                break;
            case Propagation.Mandatory:
                if (_unitOfWork.IsUsingTransaction()==false)
                    throw new Exception(nameof(Propagation.Mandatory) + "状态下未开启事务");
                break;
            default:
                throw new Exception("未定义事务Propagation属性");
                break;
        }
       
        
    }
    /// <summary>
    /// 出现异常时检查是否为忽略异常
    /// </summary>
    /// <param name="invocation"></param>
    /// <param name="ex"></param>
    private void RollbackTransWhenException(IInvocation invocation, Exception ex)
    {
        var isIgnoreEx=this.IsIgnoreException(ex);
        var uow=GetCurUnitOfWork(invocation);
        if (uow.IsUsingTransaction() == false) return;
        if (isIgnoreEx)
        {
            // 内嵌事务不需要自己提交
            if (uow.IsNestedTrans()==false)
                uow.CommitTran();
        }
        else
        {
            if(uow.IsNestedTrans())
                uow.RollbackToSavepoint("nested");
            else
            {
                uow.RollbackTran();
            }
        }
    }
    private bool IsIgnoreException(Exception ex)
    {
        var list=_useTransactionAttribute.IgnoreExceptions;
        
        return list.Any(it => ex.GetType().IsSubClassOrEqualEx(it));
    }

    private IUnitOfWork GetCurUnitOfWork(IInvocation invocation)
    {
        var callObj=invocation.InvocationTarget as IUnitOfWorkChangeable;
        return callObj.GetUnitOfWork();
    }
}