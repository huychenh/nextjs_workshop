using CarStore.AppContracts.Dtos;
using Microsoft.Extensions.Options;
using NotificationService.AppCore;
using NotificationService.AppCore.Dtos;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NotificationService.Infrastructure.Data
{
    public class Repository : INotificationRepository
    {
        private readonly string _from;

        private readonly string _fromDisplayName;

        private readonly string _subject;

        private readonly string _body;

        private readonly ISendGridClient _client;

        public Repository(IOptions<EmailConfigOptions> emailConfig
            , ISendGridClient client)
        {
            _from = emailConfig.Value.From ?? throw new ArgumentNullException(nameof(emailConfig.Value.From));
            _fromDisplayName = emailConfig.Value.FromDisplayName ?? throw new ArgumentNullException(nameof(emailConfig.Value.FromDisplayName));
            _subject = emailConfig.Value.Subject ?? throw new ArgumentNullException(nameof(emailConfig.Value.Subject));
            _body = emailConfig.Value.Body ?? throw new ArgumentNullException(nameof(emailConfig.Value.Body));

            _client = client;
        }

        public async Task<bool> SendEmail(EmailDto emailDto)
        {
            var from = new EmailAddress(_from, _fromDisplayName);
            if (!string.IsNullOrEmpty(emailDto.FromEmail))
            {
                from.Email = emailDto.FromEmail;
                from.Name = emailDto.FromEmail;
            }

            var to = new EmailAddress(emailDto.ToEmail);
            var subject = string.IsNullOrEmpty(emailDto.Subject) ? _subject : emailDto.Subject;
            var body = string.IsNullOrEmpty(emailDto.Body) ? _body : emailDto.Body;

            var msg = new SendGridMessage
            {
                From = from,
                Subject = subject,
                HtmlContent = body,
            };
            msg.AddTo(to);
            var response = await _client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }
    }
}
