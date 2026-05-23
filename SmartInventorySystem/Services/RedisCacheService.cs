using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace SmartInventorySystem.Services;

public class RedisCacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task SetDataAsync<T>(
        string key,
        T data,
        TimeSpan expirationTime)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expirationTime
        };

        var jsonData = JsonConvert.SerializeObject(data);

        await _cache.SetStringAsync(
            key,
            jsonData,
            options);
    }

    public async Task<T?> GetDataAsync<T>(string key)
    {
        var jsonData =
            await _cache.GetStringAsync(key);

        if (jsonData == null)
            return default;

        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    public async Task RemoveDataAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}