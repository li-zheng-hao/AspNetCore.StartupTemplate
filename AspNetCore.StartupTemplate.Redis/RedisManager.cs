using AspNetCore.StartUpTemplate.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AspNetCore.StartupTemplate.Redis;

public class RedisManager : IRedisManager
{
    #region 配置连接

    public string connString;
    private static ConnectionMultiplexer _db;
    private static readonly object _lock = new object();

    private readonly ILogger<RedisManager> _logger;
    public RedisManager(ILogger<RedisManager> logger)
    {
        connString = AppSettingsConstVars.RedisConn;
        _logger = logger;
    }
    
   

    /// <summary>
    /// 获取redis连接
    /// </summary>
    /// <returns></returns>
    private IDatabase GetConnectionDb()
    {
        var multiplexer = GetConnectionMultiplexer();
        // 默认使用0号数据库
        return multiplexer.GetDatabase();
    }

    /// <summary>
    /// 获取redis连接对象
    /// </summary>
    /// <returns></returns>
    private ConnectionMultiplexer GetConnectionMultiplexer()
    {
        if (_db != null && _db.IsConnected)
        {
            return _db;
        }
        lock (_lock)
        {
            var configurationOptions = GetConfigurationOptions();
            _db = ConnectionMultiplexer.Connect(configurationOptions);
            return _db;
        }
    }

    /// <summary>
    /// 获取reis配置信息
    /// </summary>
    /// <returns></returns>
    private ConfigurationOptions GetConfigurationOptions()
    {
        var configurationOptions = ConfigurationOptions.Parse(connString);
        configurationOptions.AbortOnConnectFail = false;
        configurationOptions.AllowAdmin = true;
        configurationOptions.KeepAlive = 30;
        configurationOptions.AsyncTimeout = 10 * 1000; // 10秒超时
        configurationOptions.SyncTimeout = 10 * 1000; // 10秒超时
        configurationOptions.ReconnectRetryPolicy = new LinearRetry(5000); // 重试超时5秒
        return configurationOptions;
    }

    #endregion


    #region 执行redis命令公共方法

    /// <summary>
    /// 执行redis命令公共方法
    /// </summary>
    /// <typeparam name="T">返回类型（泛型）</typeparam>
    /// <param name="func">执行的方法委托</param>
    /// <param name="actionException">异常处理委托</param>
    /// <returns></returns>
    private T Do<T>(Func<IDatabase, T> func, Func<Exception, T> actionException)
    {
        try
        {
            var database = GetConnectionDb();
            return func(database);
        }
        catch (Exception ex)
        {
            return actionException(ex);
        }
    }

    /// <summary>
    /// 执行redis命令公共方法
    /// </summary>
    /// <typeparam name="T">返回类型（泛型）</typeparam>
    /// <param name="func">执行的方法委托</param>
    /// <param name="actionException">异常处理委托</param>
    /// <returns></returns>
    private async Task<T> DoAsync<T>(Func<IDatabase, Task<T>> func, Func<Exception, T> actionException)
    {
        try
        {
            var database = GetConnectionDb();
            return await func(database);
        }
        catch (Exception ex)
        {
            return actionException(ex);
        }
    }

    #endregion

    #region Key/Value

    /// <summary>
    /// 设置Key值不设置时间
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public bool Set<T>(string key, T t)
    {
        var res = Do(database =>
        {
            var isSuccess = database.StringSet(key, ConvertJson(t));
            return isSuccess;
        }, errorInfo =>
        {
            var logObj = new { key = key, data = t };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 设置Key值不设置时间
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public async Task<bool> SetAsync<T>(string key, T t)
    {
        var res = await DoAsync(async database =>
        {
            var isSuccess = await database.StringSetAsync(key, ConvertJson(t));
            return isSuccess;
        }, errorInfo =>
        {
            var logObj = new { key = key, data = t };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }


    /// <summary>
    /// 设置Key/value单体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="expire"></param>
    /// <returns></returns>
    public bool Set(string key, string value)
    {
        var res = Do(database =>
        {
            var isSuccess = database.StringSet(key, value);
            return isSuccess;
        }, errorInfo =>
        {
            var logObj = new { key = key, value = value };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });
        return res;
    }

    /// <summary>
    /// 设置Key/value单体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="expire"></param>
    /// <returns></returns>
    public async Task<bool> SetAsync(string key, string value)
    {
        var res = await DoAsync(async database =>
        {
            var isSuccess = await database.StringSetAsync(key, value);
            return isSuccess;
        }, errorInfo =>
        {
            var logObj = new { key = key, value = value };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }


    /// <summary>
    /// 获取单体
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">键值</param>
    /// <returns></returns>
    public T Get<T>(string key)
    {
        var res = Do(database => { return ConvertObj<T>(database.StringGet(key)); }, errorInfo =>
        {
            var logObj = new { key = key };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return default(T);
        });

        return res;
    }

    /// <summary>
    /// 获取单体
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">键值</param>
    /// <returns></returns>
    public async Task<T> GetAsync<T>(string key)
    {
        var res = await DoAsync(async database => { return ConvertObj<T>(await database.StringGetAsync(key)); },
            errorInfo =>
            {
                var logObj = new { key = key };
                WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
                return default(T);
            });

        return res;
    }


    /// <summary>
    /// 获取单体
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">键值</param>
    /// <returns></returns>
    public string Get(string key)
    {
        var res = Do(database => { return database.StringGet(key); }, errorInfo =>
        {
            var logObj = new { key = key };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return RedisValue.Null;
        });

        return res;
    }

    /// <summary>
    /// 获取单体
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">键值</param>
    /// <returns></returns>
    public async Task<string> GetAsync(string key)
    {
        var res = await DoAsync(async database => { return await database.StringGetAsync(key); }, errorInfo =>
        {
            var logObj = new { key = key };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return RedisValue.Null;
        });

        return res;
    }

    /// <summary>
    /// 获取单体
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">键值</param>
    /// <returns></returns>
    public List<T> GetList<T>(IEnumerable<string> keyList)
    {
        var res = Do(database =>
        {
            var keys = keyList.Select(key => (RedisKey)key).ToArray();
            var objArray = database.StringGet(keys);
            var resInfo = ConvetList<T>(objArray);
            return resInfo;
        }, errorInfo =>
        {
            var logObj = new { keyList = keyList };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return null;
        });

        return res;
    }

    /// <summary>
    /// 获取单体
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">键值</param>
    /// <returns></returns>
    public async Task<List<T>> GetListAsync<T>(IEnumerable<string> keyList)
    {
        var res = await DoAsync(async database =>
        {
            var keys = keyList.Select(key => (RedisKey)key).ToArray();
            var objArray = await database.StringGetAsync(keys);
            var resInfo = ConvetList<T>(objArray);
            return resInfo;
        }, errorInfo =>
        {
            var logObj = new { keyList = keyList };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return null;
        });

        return res;
    }

    /// <summary>
    /// 设置Key值设置时间
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public bool Set<T>(string key, T t, TimeSpan timeSpan)
    {
        var res = Do(database => { return database.StringSet(key, ConvertJson(t), timeSpan); }, errorInfo =>
        {
            var logObj = new { key = key };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 设置Key值设置时间
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public async Task<bool> SetAsync<T>(string key, T t, TimeSpan timeSpan)
    {
        var res = await DoAsync(
            async database => { return await database.StringSetAsync(key, ConvertJson(t), timeSpan); }, errorInfo =>
            {
                var logObj = new { key = key };
                WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
                return false;
            });

        return res;
    }

    /// <summary>
    /// 判断Key是否存在
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public bool Exists(string key)
    {
        var res = Do(database => { return database.KeyExists(key); }, errorInfo =>
        {
            var logObj = new { key = key };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 判断Key是否存在
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public async Task<bool> ExistsAsync(string key)
    {
        var res = await DoAsync(async database => { return await database.KeyExistsAsync(key); }, errorInfo =>
        {
            var logObj = new { key = key };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }


    /// <summary>
    /// 设置过期时间
    /// </summary>
    /// <param name="key"></param>
    /// <param name="expire"></param>
    /// <returns></returns>
    public bool Expire(string key, TimeSpan expire)
    {
        var res = Do(database => { return database.KeyExpire(key, expire); }, errorInfo =>
        {
            var logObj = new { key = key };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 设置过期时间
    /// </summary>
    /// <param name="key"></param>
    /// <param name="expire"></param>
    /// <returns></returns>
    public async Task<bool> ExpireAsync(string key, TimeSpan expire)
    {
        var res = await DoAsync(async database => { return await database.KeyExpireAsync(key, expire); }, errorInfo =>
        {
            var logObj = new { key = key };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    #endregion

    #region Hash

    /// <summary>
    /// 判断某个数据是否已经被缓存
    /// </summary>
    /// <param name="key">hashID</param>
    /// <param name="dataKey">键值</param>
    /// <returns></returns>
    public bool Hash_Exist(string key, string dataKey)
    {
        var res = Do(database => { return database.HashExists(key, dataKey); }, errorInfo =>
        {
            var logObj = new { key = key, dataKey = dataKey };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 判断某个数据是否已经被缓存
    /// </summary>
    /// <param name="key">hashID</param>
    /// <param name="dataKey">键值</param>
    /// <returns></returns>
    public async Task<bool> Hash_ExistAsync(string key, string dataKey)
    {
        var res = await DoAsync(async database => { return await database.HashExistsAsync(key, dataKey); }, errorInfo =>
        {
            var logObj = new { key = key, dataKey = dataKey };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 存储数据到hash表需要设置失效时间
    /// </summary>
    /// <param name="key">hashID</param>
    /// <param name="dataKey">键值</param>
    /// <param name="t">实体</param>
    /// <returns></returns>
    public bool Hash_Set<T>(string key, string dataKey, T t, TimeSpan sp)
    {
        var res = Do(database =>
        {
            if (database.HashSet(key, dataKey, ConvertJson(t)))
            {
                return database.KeyExpire(key, sp);
            }
            else
            {
                return false;
            }
        }, errorInfo =>
        {
            var logObj = new { key = key, dataKey = dataKey };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 存储数据到hash表需要设置失效时间
    /// </summary>
    /// <param name="key">hashID</param>
    /// <param name="dataKey">键值</param>
    /// <param name="t">实体</param>
    /// <returns></returns>
    public async Task<bool> Hash_SetAsync<T>(string key, string dataKey, T t, TimeSpan sp)
    {
        var res = await DoAsync(async database =>
        {
            if (await database.HashSetAsync(key, dataKey, ConvertJson(t)))
            {
                return await database.KeyExpireAsync(key, sp);
            }
            else
            {
                return false;
            }
        }, errorInfo =>
        {
            var logObj = new { key = key, dataKey = dataKey };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 存储数据到hash表不需要设置失效时间
    /// </summary>
    /// <param name="key">hashID</param>
    /// <param name="dataKey">键值</param>
    /// <param name="t">实体</param>
    /// <returns></returns>
    public bool Hash_Set<T>(string key, string dataKey, T t)
    {
        var res = Do(database =>
        {
            database.HashSet(key, dataKey, ConvertJson(t));
            return true;
        }, errorInfo =>
        {
            var logObj = new { key = key, dataKey = dataKey, data = t };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 存储数据到hash表不需要设置失效时间
    /// </summary>
    /// <param name="key">hashID</param>
    /// <param name="dataKey">键值</param>
    /// <param name="t">实体</param>
    /// <returns></returns>
    public async Task<bool> Hash_SetAsync<T>(string key, string dataKey, T t)
    {
        var res = await DoAsync(async database =>
        {
            await database.HashSetAsync(key, dataKey, ConvertJson(t));
            return true;
        }, errorInfo =>
        {
            var logObj = new { key = key, dataKey = dataKey, data = t };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }


    /// <summary>
    /// 存储数据到hash表不需要设置失效时间
    /// </summary>
    /// <param name="key">hashID</param>
    /// <param name="dataKey">键值</param>
    /// <param name="t">实体</param>
    /// <returns></returns>
    public bool Hash_MSet(string key, HashEntry[] hashFields)
    {
        var res = Do(database =>
        {
            database.HashSet(key, hashFields);
            return true;
        }, errorInfo =>
        {
            var logObj = new { key = key, hashFields = hashFields };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 存储数据到hash表不需要设置失效时间
    /// </summary>
    /// <param name="key">hashID</param>
    /// <param name="dataKey">键值</param>
    /// <param name="t">实体</param>
    /// <returns></returns>
    public async Task<bool> Hash_MSetAsync(string key, HashEntry[] hashFields)
    {
        var res = await DoAsync(async database =>
        {
            await database.HashSetAsync(key, hashFields);
            return true;
        }, errorInfo =>
        {
            var logObj = new { key = key, hashFields = hashFields };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 存储数据到hash表不需要设置失效时间
    /// </summary>
    /// <param name="key">hashID</param>
    /// <param name="diclist">键值</param>
    /// <returns></returns>
    public bool Hash_MSet(string key, List<KeyValuePair<string, string>> diclist)
    {
        var res = Do(database =>
        {
            List<HashEntry> list = new List<HashEntry>();
            foreach (var item in diclist)
            {
                list.Add(new HashEntry(item.Key, item.Value));
            }

            database.HashSet(key, list.ToArray());
            return true;
        }, errorInfo =>
        {
            var logObj = new { key = key, hashFields = JsonConvert.SerializeObject(diclist) };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 存储数据到hash表不需要设置失效时间
    /// </summary>
    /// <param name="key">hashID</param>
    /// <param name="diclist">键值</param>
    /// <returns></returns>
    public async Task<bool> Hash_MSetAsync(string key, List<KeyValuePair<string, string>> diclist)
    {
        var res = await DoAsync(async database =>
        {
            List<HashEntry> list = new List<HashEntry>();
            foreach (var item in diclist)
            {
                list.Add(new HashEntry(item.Key, item.Value));
            }

            await database.HashSetAsync(key, list.ToArray());
            return true;
        }, errorInfo =>
        {
            var logObj = new { key = key, hashFields =  JsonConvert.SerializeObject(diclist) };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 存储数据到hash表需要设置失效时间
    /// </summary>
    /// <param name="key">hashID</param>
    /// <param name="dataKey">键值</param>
    /// <param name="t">实体</param>
    /// <returns></returns>
    public bool Hash_MSet(string key, HashEntry[] hashFields, TimeSpan sp)
    {
        var res = Do(database =>
        {
            database.HashSet(key, hashFields);
            return database.KeyExpire(key, sp);
        }, errorInfo =>
        {
            var logObj = new { key = key, hashFields = hashFields };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 存储数据到hash表需要设置失效时间
    /// </summary>
    /// <param name="key">hashID</param>
    /// <param name="dataKey">键值</param>
    /// <param name="t">实体</param>
    /// <returns></returns>
    public async Task<bool> Hash_MSetAsync(string key, HashEntry[] hashFields, TimeSpan sp)
    {
        var res = await DoAsync(async database =>
        {
            await database.HashSetAsync(key, hashFields);
            return await database.KeyExpireAsync(key, sp);
        }, errorInfo =>
        {
            var logObj = new { key = key, hashFields = hashFields };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 获取key数据集中所有Hashid的集合
    /// </summary>
    public List<string> GetHashKeys(string key)
    {
        var res = Do(database =>
        {
            List<string> result = new List<string>();
            RedisValue[] arr = database.HashKeys(key);
            foreach (var item in arr)
            {
                if (!item.IsNullOrEmpty)
                {
                    result.Add(item.ToString());
                }
            }

            return result;
        }, errorInfo =>
        {
            var logObj = new { key = key };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return null;
        });

        return res;
    }

    /// <summary>
    /// 获取key数据集中所有Hashid的集合
    /// </summary>
    public async Task<List<string>> GetHashKeysAsync(string key)
    {
        var res = await DoAsync(async database =>
        {
            List<string> result = new List<string>();
            RedisValue[] arr = await database.HashKeysAsync(key);
            foreach (var item in arr)
            {
                if (!item.IsNullOrEmpty)
                {
                    result.Add(item.ToString());
                }
            }

            return result;
        }, errorInfo =>
        {
            var logObj = new { key = key };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return null;
        });

        return res;
    }


    /// <summary>
    /// 获取Hash所有value集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public List<T> Hash_GetValuesList<T>(string key)
    {
        var res = Do(database =>
        {
            List<T> list = new List<T>();
            RedisValue[] arr = database.HashKeys(key);
            foreach (var item in arr)
            {
                if (!item.IsNullOrEmpty)
                {
                    string value = database.HashGet(key, item);
                    list.Add(ConvertObj<T>(value));
                }
            }

            return list;
        }, errorInfo =>
        {
            var logObj = new { key = key };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return null;
        });

        return res;
    }

    /// <summary>
    /// 获取Hash所有value集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<List<T>> Hash_GetValuesListAsync<T>(string key)
    {
        var res = await DoAsync(async database =>
        {
            List<T> list = new List<T>();
            RedisValue[] arr = await database.HashKeysAsync(key);
            foreach (var item in arr)
            {
                if (!item.IsNullOrEmpty)
                {
                    string value = await database.HashGetAsync(key, item);
                    list.Add(ConvertObj<T>(value));
                }
            }

            return list;
        }, errorInfo =>
        {
            var logObj = new { key = key };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return null;
        });

        return res;
    }


    /// <summary>
    /// 移除hash中的某值
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="fields">键值</param>
    /// <returns></returns>
    public bool Hash_Remove(string key, string fields)
    {
        var res = Do(database => { return database.HashDelete(key, fields); }, errorInfo =>
        {
            var logObj = new { key = key, fields = fields };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 移除hash中的某值
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="fields">键值</param>
    /// <returns></returns>
    public async Task<bool> Hash_RemoveAsync(string key, string fields)
    {
        var res = await DoAsync(async database => { return await database.HashDeleteAsync(key, fields); }, errorInfo =>
        {
            var logObj = new { key = key, fields = fields };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }


    /// <summary>
    /// 根据Hash键中的field获取数据string类型
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="field">键值</param>
    /// <returns></returns>
    public T Hash_GetEntity<T>(string key, string field)
    {
        var res = Do(database => { return ConvertObj<T>(database.HashGet(key, field)); }, errorInfo =>
        {
            var logObj = new { key = key, field = field };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return default(T);
        });

        return res;
    }

    /// <summary>
    /// 根据Hash键中的field获取数据string类型
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="field">键值</param>
    /// <returns></returns>
    public async Task<T> Hash_GetEntityAsync<T>(string key, string field)
    {
        var res = await DoAsync(async database => { return ConvertObj<T>(await database.HashGetAsync(key, field)); },
            errorInfo =>
            {
                var logObj = new { key = key, field = field };
                WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
                return default(T);
            });

        return res;
    }

    /// <summary>
    /// 根据Hash键中的field获取数据string类型
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="fields">键值</param>
    /// <returns></returns>
    public T[] Hash_GetList<T>(string key, string[] fields)
    {
        var res = Do(database =>
        {
            List<RedisValue> list = new List<RedisValue>();
            foreach (var item in fields.ToList())
            {
                list.Add(item);
            }

            var result = database.HashGet(key, list.ToArray());
            if (result.Count() > 0)
            {
                List<T> TList = new List<T>();
                foreach (var item in result.ToList())
                {
                    TList.Add(ConvertObj<T>(item));
                }

                return TList.ToArray();
            }
            else
            {
                return null;
            }
        }, errorInfo =>
        {
            var logObj = new { key = key, fields = fields };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return null;
        });

        return res;
    }

    /// <summary>
    /// 根据Hash键中的field获取数据string类型
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="fields">键值</param>
    /// <returns></returns>
    public async Task<T[]> Hash_GetListAsync<T>(string key, string[] fields)
    {
        var res = await DoAsync(async database =>
        {
            List<RedisValue> list = new List<RedisValue>();
            foreach (var item in fields.ToList())
            {
                list.Add(item);
            }

            var result = await database.HashGetAsync(key, list.ToArray());
            if (result.Count() > 0)
            {
                List<T> TList = new List<T>();
                foreach (var item in result.ToList())
                {
                    TList.Add(ConvertObj<T>(item));
                }

                return TList.ToArray();
            }
            else
            {
                return null;
            }
        }, errorInfo =>
        {
            var logObj = new { key = key, fields = fields };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return null;
        });

        return res;
    }

    /// <summary>
    /// 移除指定数据缓存
    /// </summary>
    /// <param name="key">键</param>
    public bool Remove(string key)
    {
        var res = Do(database => { return database.KeyDelete(key); }, errorInfo =>
        {
            var logObj = new { key = key };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    /// <summary>
    /// 移除指定数据缓存
    /// </summary>
    /// <param name="key">键</param>
    public async Task<bool> RemoveAsync(string key)
    {
        var res = await DoAsync(async database => { return await database.KeyDeleteAsync(key); }, errorInfo =>
        {
            var logObj = new { key = key };
            WriteSqlErrorLog(JsonConvert.SerializeObject(logObj), errorInfo);
            return false;
        });

        return res;
    }

    #endregion

    #region Json操作

    /// <summary>
    /// 序列化
    /// </summary>
    /// <typeparam name="T">类</typeparam>
    /// <param name="value">值</param>
    /// <returns></returns>
    private string ConvertJson<T>(T value)
    {
        string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
        return result;
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    private T ConvertObj<T>(RedisValue value)
    {
        if (value.IsNullOrEmpty)
        {
            return default;
        }

        if (typeof(T) == typeof(string))
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        return JsonConvert.DeserializeObject<T>(value);
    }

    private List<T> ConvetList<T>(RedisValue[] values)
    {
        List<T> result = new List<T>();
        foreach (var item in values)
        {
            if (!item.IsNullOrEmpty)
            {
                var model = ConvertObj<T>(item);
                result.Add(model);
            }
        }

        return result;
    }

    private RedisKey[] ConvertRedisKeys(List<string> redisKeys)
    {
        return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
    }

    #endregion

    #region 错误重载

    /// <summary>
    /// 写入错误日志
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="ex"></param>
    public void WriteSqlErrorLog(string parameter, Exception ex)
    {
    }

    /// <summary>
    /// 写入错误日志
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="ex"></param>
    public void WriteSqlErrorLog(Exception ex)
    {
    }

    #endregion
}