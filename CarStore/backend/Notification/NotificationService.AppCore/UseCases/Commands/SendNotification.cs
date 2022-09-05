using CarStore.AppContracts.Dtos;
using MediatR;
using N8T.Core.Domain;
using FluentValidation;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NotificationService.AppCore.UseCases.Commands
{
    public class SendNotification
    {
        public record Command : ICreateCommand<NotificationDto, string>
        {
            public NotificationDto Model { get; init; } = default!;

            internal class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(v => v.Model.To)
                        .NotEmpty();

                    RuleFor(v => v.Model.Subject)
                        .NotEmpty();

                    RuleFor(v => v.Model.Body)
                        .NotEmpty();
                }
            }

            internal class Handler : IRequestHandler<Command, ResultModel<string>>
            {
                private readonly ISendGridClient _sendGridClient;

                public Handler(ISendGridClient sendGridClient)
                {
                    _sendGridClient = sendGridClient; 
                }

                public async Task<ResultModel<string>> Handle(Command request, CancellationToken cancellationToken)
                {
                    var msg = new SendGridMessage()
                    {
                        From = new EmailAddress(request.Model.From, request.Model.FromName),
                        Subject = request.Model.Subject,
                        HtmlContent = request.Model.Body,
                    };
                    msg.AddTo(request.Model.To);

                    var response = await _sendGridClient.SendEmailAsync(msg);
                    
                    string message = response.IsSuccessStatusCode 
                        ? "Email Send Successfully" : "Email Sending Failed";

                    return ResultModel<string>.Create(message);
                }
            }
        }
    }
}
