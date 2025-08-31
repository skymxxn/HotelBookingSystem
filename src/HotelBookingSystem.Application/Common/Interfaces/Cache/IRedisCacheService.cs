namespace HotelBookingSystem.Application.Common.Interfaces.Cache;

public interface IRedisCacheService
{
    Task<T?> GetDataAsync<T>(string key);
    Task SetDataAsync<T>(string key, T data);
}
