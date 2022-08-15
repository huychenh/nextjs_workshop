using CarStore.AppContracts.Dtos;
using CarStore.IntegrationEvents.Notification;
using CarStore.IntegrationEvents.Ordering;
using MediatR;
using Microsoft.Extensions.Options;
using N8T.Infrastructure.Bus;
using Newtonsoft.Json;
using NotificationService.AppCore.UseCases.Commands;

namespace NotificationService.Api.Services
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly ILogger<NotificationBackgroundService> _logger;

        private readonly IEventBus _eventBus;

        private readonly BackgroundServiceOptions _options;

        private readonly ISender _mediator;

        public NotificationBackgroundService(IEventBus eventBus
            , ILogger<NotificationBackgroundService> logger
            , IOptions<BackgroundServiceOptions> options
            , ISender mediator)
        {
            _eventBus = eventBus;
            _logger = logger;
            _options = options.Value;
            _mediator = mediator;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            new Thread(async () => await StartConsumerLoop(_options.MilisecondsDelay, stoppingToken)).Start();

            return Task.CompletedTask;
        }

        private async Task StartConsumerLoop(int milisecondsDelay, CancellationToken stoppingToken)
        {
            stoppingToken.Register(() =>
                _logger.LogInformation($"{nameof(NotificationBackgroundService)} background task is stopping."));

            _logger.LogInformation($"{nameof(NotificationBackgroundService)} is starting.");

            _eventBus
                .Subscribe<NotificationIntegrationEvent, IIntegrationEventHandler<NotificationIntegrationEvent>>(
                    (new NotificationIntegrationEvent()).Topics
                );

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(NotificationBackgroundService)} task doing background work.");

                try
                {
                    var msg = _eventBus.Consume<NotificationIntegrationEvent>();
                    SendEmailCommand(msg, stoppingToken);

                    await Task.Delay(_options.MilisecondsDelay, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"{nameof(NotificationBackgroundService)} exception:{e.Message}");
                }

                _logger.LogInformation($"{nameof(NotificationBackgroundService)} background task is stopping.");
            }
        }

        private void SendEmailCommand(NotificationIntegrationEvent msg, CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(SendEmailCommand)} for:{JsonConvert.SerializeObject(msg)}.");

            var command = new SendEmail.Command
            {
                Model = new EmailDto
                {
                    FromEmail = msg.From,
                    ToEmail = msg.To,
                    Subject = msg.Subject,
                    Body = msg.Body,
                }
            };
            _mediator.Send(command, stoppingToken);
        }
    }

    public class  BackgroundServiceOptions
    {
        public int MilisecondsDelay { get; set; } = 0;
    }
}
