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
        var options = new DistributedCacheEntryOptions//تحديد خيارات التخزين في Redis
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(30)
        };

        //  هون serialization
        var json = JsonSerializer.Serialize(value);//تحويل الكائن إلى JSON string

        await _cache.SetStringAsync(key, json, options);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await _cache.GetStringAsync(key);

        if (json == null)
            return default;

        //  هون deserialization
        return JsonSerializer.Deserialize<T>(json);//تحويل جيسون الى كائن من نوع T
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}