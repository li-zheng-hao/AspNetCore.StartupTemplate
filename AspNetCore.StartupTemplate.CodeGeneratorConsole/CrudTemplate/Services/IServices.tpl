
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace {{NameSpacePrefix}}.IServices
{
	/// <summary>
    /// {{ModelDescription}} 服务工厂接口
    /// </summary>
    [Intercept(typeof(TransactionInterceptor))]
    public interface I{{ModelClassName}}Services
    {
        
    }
}
