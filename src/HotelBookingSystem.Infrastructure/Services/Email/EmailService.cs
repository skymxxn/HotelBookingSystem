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
        var link = $"{_options.FrontendUrl}/verify-email?token={token}";
        var body = $"""
                        <p>Hello!</p>
                        <p>To confirm your email, click the link below:</p>
                        <p><a href="{link}">Confirm Email</a></p>
                    """;

        await SendEmailAsync(email, "Confirm your email", body);
        _logger.LogInformation("Confirmation email sent to {Email}", email);
    }
}