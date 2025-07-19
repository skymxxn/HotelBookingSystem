namespace HotelBookingSystem.Application.Common.Interfaces.Email;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
    Task SendEmailConfirmationAsync(string email, string token);
}