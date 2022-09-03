using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartUpTemplate.Model;
using FreeSql;
using Microsoft.Extensions.Logging;

namespace {{NameSpacePrefix}}.Services
{
    /// <summary>
    /// {{ModelDescription}} 接口实现
    /// </summary>
    public class {{ModelClassName}}Services : I{{ModelClassName}}Services
    {
        private readonly I{{ModelClassName}}Repository _dal;
        private readonly ILogger _logger;

        public {{ModelClassName}}Services(ILogger<{{ModelClassName}}Service> logger, I{{ModelClassName}}Repository dal)
        {
            this._dal = dal;
            _unitOfWork = unitOfWork;
            base.BaseRepo = userRepository;
        }
    }
}
