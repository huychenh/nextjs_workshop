
using NotificationService.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NotificationService.Services.EmailService
{
    public class EmailService : IEmailService
    {
        //private readonly ISendGridClient _sendGridClient;
        //public EmailService(ISendGridClient sendGridClient)
        //{
        //    //_sendGridClient = sendGridClient;
        //}

        public async Task<string> SendEmail(EmailDto request)
        {
            #region sendgrid
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(request.FromEmail, request.FromName),
                Subject = request.Subject,
                PlainTextContent = request.Body
            };
            msg.AddTo(request.To);
            var response = await client.SendEmailAsync(msg);
            string message = response.IsSuccessStatusCode ? "Email Send Successfully" :
            "Email Sending Failed";
            return message;
            #endregion
        }
    }
}
