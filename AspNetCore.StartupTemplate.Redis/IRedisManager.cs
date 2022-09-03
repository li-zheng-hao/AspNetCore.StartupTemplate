using AspNetCore.StartUpTemplate.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AspNetCore.StartupTemplate.Redis;

public interface IRedisManager
{
    bool Set<T>(string key, T t);
    Task<bool> SetAsync<T>(string key, T t);

    bool Set(string key, string value);


    Task<bool> SetAsync(string key, string value);


    T Get<T>(string key);


    Task<T> GetAsync<T>(string key);


    string Get(string key);


    Task<string> GetAsync(string key);

    List<T> GetList<T>(IEnumerable<string> keyList);


    Task<List<T>> GetListAsync<T>(IEnumerable<string> keyList);


    bool Set<T>(string key, T t, TimeSpan timeSpan);


    Task<bool> SetAsync<T>(string key, T t, TimeSpan timeSpan);


    bool Exists(string key);


    Task<bool> ExistsAsync(string key);


    bool Expire(string key, TimeSpan expire);


    Task<bool> ExpireAsync(string key, TimeSpan expire);


    bool Hash_Exist(string key, string dataKey);


    Task<bool> Hash_ExistAsync(string key, string dataKey);


    bool Hash_Set<T>(string key, string dataKey, T t, TimeSpan sp);


    Task<bool> Hash_SetAsync<T>(string key, string dataKey, T t, TimeSpan sp);


    bool Hash_Set<T>(string key, string dataKey, T t);


    Task<bool> Hash_SetAsync<T>(string key, string dataKey, T t);


    bool Hash_MSet(string key, HashEntry[] hashFields);


    Task<bool> Hash_MSetAsync(string key, HashEntry[] hashFields);


    bool Hash_MSet(string key, List<KeyValuePair<string, string>> diclist);


    Task<bool> Hash_MSetAsync(string key, List<KeyValuePair<string, string>> diclist);


    bool Hash_MSet(string key, HashEntry[] hashFields, TimeSpan sp);


    Task<bool> Hash_MSetAsync(string key, HashEntry[] hashFields, TimeSpan sp);


    List<string> GetHashKeys(string key);


    Task<List<string>> GetHashKeysAsync(string key);


    List<T> Hash_GetValuesList<T>(string key);


    Task<List<T>> Hash_GetValuesListAsync<T>(string key);


    bool Hash_Remove(string key, string fields);


    Task<bool> Hash_RemoveAsync(string key, string fields);


    T Hash_GetEntity<T>(string key, string field);


    Task<T> Hash_GetEntityAsync<T>(string key, string field);


    T[] Hash_GetList<T>(string key, string[] fields);


    Task<T[]> Hash_GetListAsync<T>(string key, string[] fields);


    bool Remove(string key);

    /// <summary>
    /// 移除指定数据缓存
    /// </summary>
    /// <param name="key">键</param>
    Task<bool> RemoveAsync(string key);


  
    void WriteSqlErrorLog(string parameter, Exception ex);

   
    void WriteSqlErrorLog(Exception ex);
}