namespace HotelBookingSystem.Application.Common.Interfaces.Users;

public interface ICurrentUserService 
{
    Guid GetUserId();
    List<string> GetRoles();
}