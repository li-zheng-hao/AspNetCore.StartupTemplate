using System.ComponentModel;
using AspNetCore.CacheOutput;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Contract;
using AspNetCore.StartUpTemplate.Core;
using AspNetCore.StartUpTemplate.Core.Transaction;
using AspNetCore.StartUpTemplate.Filter;
using AspNetCore.StartUpTemplate.IRepository;
using AspNetCore.StartUpTemplate.IService;
using AspNetCore.StartUpTemplate.Model;
using AspNetCore.StartupTemplate.Snowflake.SnowFlake;
using Dtmcli;
using FreeSql;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.StartUpTemplate.Webapi.Controllers;

/// <summary>
/// DTM分布式事务相关示例 
/// </summary>
[ApiController]
[Route("[controller]")]
public class DtmController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly IDtmClient _dtmClient;
    private readonly IDtmTransFactory _transFactory;
    private IBranchBarrierFactory _barrierFactory;
    private readonly UnitOfWorkManager _uowManager;
    private readonly GlobalConfig _config;


    public DtmController(ILogger<UserController> logger, IUserService us
        , IDtmClient client, IDtmTransFactory transFactory, IBranchBarrierFactory barrierFactory,
        UnitOfWorkManager manager,GlobalConfig config)
    {
        _logger = logger;
        _config = config;
        _userService = us;
        _dtmClient = client;
        _transFactory = transFactory;
        _barrierFactory = barrierFactory;
        _uowManager = manager;
    }

    /// <summary>
    /// Saga失败回滚示例
    /// </summary>
    [HttpPost("SagaErrorDemo")]
    public async Task<IActionResult> SagaErrorDemo(CancellationToken cancellationToken)
    {
        try
        {
            var test = _config.Dtm.BusiUrl + "Dtm/SagaIn";
            _logger.LogWarning("==========开始测试Saga失败直接回滚的示例代码===========");
            var gid = await _dtmClient.GenGid(cancellationToken);
            var saga = _transFactory.NewSaga(gid)
                .Add("http://localhost:5000/Dtm/SagaOut", "http://localhost:5000/Dtm/SagaOutRevert",
                    new TransRequest(2, -100))
                .Add("http://localhost:5000/Dtm/SagaInError",
                    "http://localhost:5000/Dtm/SagaInRevert",
                    new TransRequest(1, 100));
            // saga.EnableWaitResult();
            await saga.Submit(cancellationToken);
        }
        catch (System.Exception ex)
        {
            return BadRequest("出现异常了，这里自己写业务消息");
        }

        return Ok("成功处理");
    }

    /// <summary>
    /// 转入分支
    /// dtm调用 不要自己调用
    /// </summary>
    [HttpPost("SagaInError")]
    [SwaggerIgnore]
    public async Task<IActionResult> SagaInError([FromBody] TransRequest body)
    {
        try
        {
            var branchBarrier = _barrierFactory.CreateBranchBarrier(Request.Query);
            var uow = _uowManager.Begin();
            var trans = uow.GetOrBeginTransaction();
            await branchBarrier.Call(trans, async (tx) =>
            {
                _logger?.LogWarning("用户: {0},转入 {1} 元---转入操作 屏障内", body.UserId, body.Number);
                _userService.ChangeMoneyError(body.UserId, body.Number);
                await Task.CompletedTask;
            });

            return Ok(TransResponse.BuildSucceedResponse());
        }
        catch (Exception e)
        {
            _logger.LogError("转入出现异常了");
            return BadRequest(TransResponse.BuildFailureResponse());
        }
    }

    /// <summary>
    /// 转出分支
    /// dtm调用 不要自己调用
    /// saga子分支失败直接回滚
    /// </summary>
    [HttpPost("SagaOut")]
    [SwaggerIgnore]
    public async Task<IActionResult> SagaOut([FromBody] TransRequest body)
    {
        var branchBarrier = _barrierFactory.CreateBranchBarrier(Request.Query);
        var uow = _uowManager.Begin();
        var trans = uow.GetOrBeginTransaction();
        await branchBarrier.Call(trans, async (tx) =>
        {
            _logger?.LogWarning("用户: {0},转出 {1} 元---转出操作 屏障内", body.UserId, body.Number);
            _userService.ChangeMoney(body.UserId, body.Number);

            await Task.CompletedTask;
        });
        return Ok(TransResponse.BuildSucceedResponse());
    }

    /// <summary>
    /// dtm调用 不要自己调用
    /// </summary>
    [HttpPost("SagaInRevert")]
    [SwaggerIgnore]
    public async Task<IActionResult> SagaInErrorRevert([FromBody] TransRequest body)
    {
        var branchBarrier = _barrierFactory.CreateBranchBarrier(Request.Query);
        var uow = _uowManager.Begin();
        var trans = uow.GetOrBeginTransaction();
        await branchBarrier.Call(trans, async (tx) =>
        {
            _logger?.LogWarning("用户: {0},转出 {1} 元---转入操作回滚 屏障内", body.UserId, -1 * body.Number);
            _userService.ChangeMoney(body.UserId, -1 * body.Number);
            await Task.CompletedTask;
        });


        return Ok(TransResponse.BuildSucceedResponse());
    }

    /// <summary>
    /// dtm调用 不要自己调用
    /// saga子分支失败直接回滚
    /// </summary>
    [HttpPost("SagaOutRevert")]
    [SwaggerIgnore]
    public async Task<IActionResult> SagaOutRevert([FromBody] TransRequest body)
    {
        var branchBarrier = _barrierFactory.CreateBranchBarrier(Request.Query);
        var uow = _uowManager.Begin();
        var trans = uow.GetOrBeginTransaction();
        await branchBarrier.Call(trans, async (tx) =>
        {
            _logger?.LogWarning("用户: {0},转出 {1} 元---转出操作回滚 屏障内", body.UserId, 1 * body.Number);
            _userService.ChangeMoney(body.UserId, -1 * body.Number);
            await Task.CompletedTask;
        });

        return Ok(TransResponse.BuildSucceedResponse());
    }
}