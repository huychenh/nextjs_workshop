using NotificationService.Models;

namespace NotificationService.Services.EmailService
{
    public interface IEmailService
    {
        Task<string> SendEmail(EmailDto request);
    }
}
