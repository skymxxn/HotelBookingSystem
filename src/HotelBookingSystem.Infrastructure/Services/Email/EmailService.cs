using HotelBookingSystem.Application.Common.Interfaces.Email;
using HotelBookingSystem.Infrastructure.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace HotelBookingSystem.Infrastructure.Services.Email;

public class EmailService  : IEmailService
{
    private readonly SmtpOptions _options;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<SmtpOptions> options, ILogger<EmailService> logger)
    {
        _logger = logger;
        _options = options.Value;
    }
    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_options.SenderName, _options.SenderEmail));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        email.Body = new TextPart(isHtml ? "html" : "plain")
        {
            Text = body
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_options.Server, _options.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_options.Username, _options.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

    public async Task SendEmailConfirmationAsync(string email, string token)
    {
        var link = $"{_options.FrontendUrl}auth/verify-email?token={token}";
        var subject = "Confirm your email";

        var templatePath = Path.Combine(AppContext.BaseDirectory, "Services", "Email", "Templates", "Emails", "EmailConfirmationTemplate.html");
        var template = await File.ReadAllTextAsync(templatePath);
        var body = template.Replace("{Link}", link);

        await SendEmailAsync(email, subject, body);
        _logger.LogInformation("Confirmation email sent to {Email}", email);
    }
    
    public async Task SendPasswordResetAsync(string email, string token)
    {
        var link = $"{_options.FrontendUrl}auth/reset-password?token={token}";
        var subject = "Reset your password";

        var templatePath = Path.Combine(AppContext.BaseDirectory, "Services", "Email", "Templates", "Emails", "PasswordResetTemplate.html");
        var template = await File.ReadAllTextAsync(templatePath);
        var body = template.Replace("{Link}", link);

        await SendEmailAsync(email, subject, body);
        _logger.LogInformation("Password reset email sent to {Email}", email);
    }
    
    public async Task SendBookingConfirmationAsync(string email, string roomName, DateTime startDate, DateTime endDate, string token)
    {
        var link = $"{_options.FrontendUrl}bookings/confirm-booking?token={token}";
        var subject = "Confirm your booking";

        var templatePath = Path.Combine(AppContext.BaseDirectory, "Services", "Email", "Templates", "Emails", "BookingConfirmationTemplate.html");
        var template = await File.ReadAllTextAsync(templatePath);

        var body = template
            .Replace("{RoomName}", roomName)
            .Replace("{StartDate}", startDate.ToString("yyyy-MM-dd"))
            .Replace("{EndDate}", endDate.ToString("yyyy-MM-dd"))
            .Replace("{Link}", link);

        await SendEmailAsync(email, subject, body);
        _logger.LogInformation("Booking confirmation email sent to {Email}", email);
    }
}