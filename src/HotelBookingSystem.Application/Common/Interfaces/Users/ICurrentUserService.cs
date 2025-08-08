namespace HotelBookingSystem.Application.Common.Interfaces.Users;

public interface ICurrentUserService 
{
    Guid GetUserId();
    List<string> GetRoles();
    string GetUserEmail();
    bool IsAuthenticated();
    bool IsAdmin();
    bool IsModerator();
    bool IsManager();
    bool IsUser();
}