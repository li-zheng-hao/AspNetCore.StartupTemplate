using System.Data;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.IService;
using Dapper;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace AspNetCore.StartUpTemplate.Webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CapController : ControllerBase
    {
        private readonly ICapPublisher _capBus;
        private readonly IUserService _userService;

        public CapController(ICapPublisher capPublisher,IUserService userService)
        {
            _userService = userService;
            _capBus = capPublisher;
        }

        [HttpGet]
        public async Task<IActionResult> WithoutTransaction()
        {
            await _capBus.PublishAsync("sample.rabbitmq.mysql", DateTime.Now);

            return Ok();
        }
        /// <summary>
        /// 结合FreeSql特性事务提交示例
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PublishWithFreeSqlOk()
        {
            await _userService.CapWithFreeSqlTrans();
            return Ok();
        }
        /// <summary>
        /// 结合FreeSql特性事务提交失败回滚示例
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PublishWithFreeSqlError()
        {
            await _userService.CapWithFreeSqlTransRollBack();
            return Ok();
        }
        

        [NonAction]
        [CapSubscribe(MqTopicConfig.CAP_DEFAULT_TOPIC)]
        public void Subscriber(DateTime p)
        {
            Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {p}");
        }

        [NonAction]
        [CapSubscribe(MqTopicConfig.CAP_DEFAULT_TOPIC, Group = "group.test2")]
        public void Subscriber2(DateTime p, [FromCap]CapHeader header)
        {
            Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {p}");
        }
    }
}
