using CarStore.AppContracts.Dtos;
using FluentValidation;
using MediatR;
using N8T.Core.Domain;

namespace NotificationService.AppCore.UseCases.Commands
{
    public class SendEmail
    {
        public record Command : ICreateCommand<EmailDto, string>
        {
            public EmailDto Model { get; init; } = default!;

            internal class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(v => v.Model.ToEmail)
                        .NotEmpty().WithMessage("ToEmail is required.");

                    RuleFor(v => v.Model.Subject)
                        .NotEmpty().WithMessage("Subject is required.");

                    RuleFor(v => v.Model.Body)
                        .NotEmpty().WithMessage("Body is required.");
                }
            }

            internal class Handler : IRequestHandler<Command, ResultModel<string>>
            {
                private readonly INotificationRepository _repository;

                public Handler(INotificationRepository repository)
                {
                    _repository = repository ?? throw new ArgumentNullException(nameof(INotificationRepository));
                }

                public async Task<ResultModel<string>> Handle(Command request, CancellationToken cancellationToken)
                {
                    await _repository.SendEmail(request.Model);

                    return ResultModel<string>.Create(string.Empty);
                }
            }
        }
    }
}
