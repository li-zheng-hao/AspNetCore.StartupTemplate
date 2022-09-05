using System.Timers;
using AspNetCore.StartupTemplate.Snowflake.SnowFlake;
using Microsoft.Extensions.Logging;

namespace AspNetCore.StartupTemplate.Redis;
/// <summary>
/// 分布式锁，支持自动续期
/// </summary>
public class DistributeLock:IDisposable
{
   private bool _disposed;
   private readonly IRedisManager _redisManager;
   private System.Timers.Timer renewTimer;
   private readonly ISnowflakeIdMaker _snowflakeMaker;
   private long _uniqueUserToken;
   private string _uniqueKey;
   private readonly ILogger<DistributeLock> _logger;

   public DistributeLock(IRedisManager redisManager,ISnowflakeIdMaker snowflakeMarker,ILogger<DistributeLock> logger)
   {
      _logger = logger;
      _redisManager = redisManager;
      _snowflakeMaker = snowflakeMarker;
      renewTimer = new System.Timers.Timer();
      renewTimer.Elapsed += RenewLockLifeTime;
      _uniqueUserToken = snowflakeMarker.NextId();

   }
   private void RenewLockLifeTime(object? sender, ElapsedEventArgs e)
   {
      var renewed=_redisManager.RenewLock(_uniqueKey.ToString(), _uniqueUserToken.ToString());
      if (renewed == false)
      {
         _logger.LogError($"{_uniqueKey}-{_uniqueUserToken}的锁无法续期!请检查是否存在任何异常");
      }
   }
   /// <summary>
   /// 当前类只允许获取一次
   /// value为本类实例获取的任意一个随机雪花id 为用户标识
   /// </summary>
   /// <param name="uniqueKey">key根据业务需求设置</param>
   /// <param name="exipreTime"></param>
   /// <returns></returns>
   public bool RequireCurLock(string uniqueKey,int exipreTime=10)
   {
      if (string.IsNullOrWhiteSpace(_uniqueKey) == false)
         throw new Exception("当前类只允许获取一次锁，如果需要获取另一把锁请重新创建本类实例");
      _uniqueKey = uniqueKey;
      var locked=_redisManager.Lock(uniqueKey, _uniqueUserToken.ToString(),exipreTime);
      if (locked)
      {
         // ex：10秒过期，8秒续期 
         // 可优化 这里获取之后立刻又会去刷新一次
         renewTimer.Interval = Math.Floor( exipreTime * 0.8);
         renewTimer.Start();
      }
      return locked;
   }
  
   public void Dispose()
   {
      // Dispose of unmanaged resources.
      Dispose(true);
      // Suppress finalization.
      GC.SuppressFinalize(this);
   }
   protected virtual void Dispose(bool disposing)
   {
      if (_disposed)
      {
         return;
      }

      if (disposing)
      {
         // TODO: dispose managed state (managed objects).
         if(_redisManager.Get<string>(_uniqueKey)==_uniqueUserToken.ToString())
            _redisManager.ReleaseLock(_uniqueKey,_uniqueUserToken.ToString());
         renewTimer.Stop();
         renewTimer = null;
      }
      // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
      
      // TODO: set large fields to null.
      _disposed = true;
   }
}