namespace HotelBookingSystem.Infrastructure.Options;

public class SmtpOptions
{
    public string Server { get; set; } = default!;
    public int Port { get; set; }
    public string Username { get; set; } = default!;
    public string Password { get; set; } =  default!;
    public string SenderEmail { get; set; } = default!;
    public string SenderName { get; set; } = default!;
    public string FrontendUrl { get; set; } = default!;
}