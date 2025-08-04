namespace HotelBookingSystem.Application.Common.Interfaces.Cache;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task<T> SetAsync<T>(string key, T value, TimeSpan absoluteExpirationRelativeToNow);
    void Remove(string key);
}
