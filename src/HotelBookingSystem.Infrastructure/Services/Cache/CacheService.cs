using HotelBookingSystem.Application.Common.Interfaces.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace HotelBookingSystem.Infrastructure.Services.Cache;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        return await Task.FromResult(_cache.TryGetValue(key, out T? value) ? value : default);
    }

    public async Task<T> SetAsync<T>(string key, T value, TimeSpan absoluteExpirationRelativeToNow)
    {
        _cache.Set(key, value, absoluteExpirationRelativeToNow);
        return await Task.FromResult(value);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}
