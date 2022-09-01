using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace {{NameSpacePrefix}}.Repository;
/// <summary>
/// {{ModelDescription}} 接口实现
/// </summary>
public class {{ModelClassName}}Repository : CurBaseRepository<{{ModelClassName}},long>, I{{ModelClassName}}Repository
{
    public {{ModelClassName}}Repository(IFreeSql fsql) : base(fsql)
    {
    }
}


