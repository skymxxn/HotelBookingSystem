using System.Text.Json;
using HotelBookingSystem.Application.Common.Interfaces.Cache;
using Microsoft.Extensions.Caching.Distributed;

namespace HotelBookingSystem.Infrastructure.Services.Cache;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache? _cache;

    public RedisCacheService(IDistributedCache? cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetDataAsync<T>(string key)
    {
        var data = await _cache?.GetStringAsync(key)!;
        
        if (data is null)
            return default(T);
        
        return JsonSerializer.Deserialize<T>(data);
    }

    public async Task SetDataAsync<T>(string key, T data)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };
        
        await _cache?.SetStringAsync(key, JsonSerializer.Serialize(data), options)!;
    }
}
