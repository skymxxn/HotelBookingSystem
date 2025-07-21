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
        var link = $"{_options.BackendUrl}auth/verify-email?token={token}";
        var subject = $"Confirm your email";
        var body = $"""
                        <p>Hello!</p>
                        <p>To confirm your email, click the link below:</p>
                        <p><a href="{link}">Confirm Email</a></p>
                    """;

        await SendEmailAsync(email, subject, body);
        _logger.LogInformation("Confirmation email sent to {Email}", email);
    }
    
    public async Task SendPasswordResetAsync(string email, string token)
    {
        var link = $"{_options.BackendUrl}auth/reset-password?token={token}";
        var subject = $"Reset your password";
        var body = $"""
                        <p>Hello!</p>
                        <p>To reset your password, click the link below:</p>
                        <p><a href="{link}">Reset password</a></p>
                    """;

        await SendEmailAsync(email, subject, body);
        _logger.LogInformation("Password reset email sent to {Email}", email);
    }

    public async Task SendBookingCreatedAsync(string email, string roomName, DateTime startDate, DateTime endDate)
    {
        var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "Emails",  "BookingCreatedTemplate.html");
        var template = await File.ReadAllTextAsync(templatePath);

        var body = template
            .Replace("{RoomName}", roomName)
            .Replace("{StartDate}", startDate.ToString("yyyy-MM-dd"))
            .Replace("{EndDate}", endDate.ToString("yyyy-MM-dd"));
        
        await SendEmailAsync(email, "BookingCreated", body);
    }
    
    public async Task SendBookingConfirmationAsync(string email, string token)
    {
        var link = $"{_options.BackendUrl}bookings/confirm-booking?token={token}";
        var subject = "Confirm your booking";
        var body = $"""
                        <p>Hello!</p>
                        <p>Please confirm your booking by clicking the link below:</p>
                        <p><a href="{link}">Confirm Booking</a></p>
                    """;

        await SendEmailAsync(email, subject, body);
        _logger.LogInformation("Booking confirmation email sent to {Email}", email);
    }
}