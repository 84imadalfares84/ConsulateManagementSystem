using Consulate.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task SetAsync(string key, object value, TimeSpan? expiry = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(30)
        };

        // 🔥 هون serialization
        var json = JsonSerializer.Serialize(value);

        await _cache.SetStringAsync(key, json, options);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await _cache.GetStringAsync(key);

        if (json == null)
            return default;

        // 🔥 هون deserialization
        return JsonSerializer.Deserialize<T>(json);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}