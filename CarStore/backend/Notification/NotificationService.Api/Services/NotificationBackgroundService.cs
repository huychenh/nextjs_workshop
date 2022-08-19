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
        private readonly IEventBus _eventBus;

        private readonly BackgroundServiceOptions _options;

        private readonly ISender _mediator;

        private IServiceProvider Services { get; }

        public NotificationBackgroundService(IEventBus eventBus
            , IOptions<BackgroundServiceOptions> options
            , IServiceProvider services)
        {
            _eventBus = eventBus;
            _options = options.Value;
            Services = services;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = Services.CreateScope();
            var logger = scope.ServiceProvider
                        .GetRequiredService<ILogger<NotificationBackgroundService>>();
            var mediator = scope.ServiceProvider
                        .GetRequiredService<ISender>();

            new Thread(async () => await StartConsumerLoop(_options.MilisecondsDelay,
                logger, mediator, stoppingToken)).Start();

            return Task.CompletedTask;
        }

        private async Task StartConsumerLoop(int milisecondsDelay,
            ILogger<NotificationBackgroundService> logger,
            ISender mediator, CancellationToken stoppingToken)
        {
            stoppingToken.Register(() =>
                logger.LogInformation($"{nameof(NotificationBackgroundService)} background task is stopping."));

            logger.LogInformation($"{nameof(NotificationBackgroundService)} is starting.");

            //HACK: Pre-create Notification Topic
            var notificationEvent = new NotificationIntegrationEvent();
            await _eventBus.PublishAsync(notificationEvent, notificationEvent.Topics, stoppingToken);
            
            _eventBus
                .Subscribe<NotificationIntegrationEvent, IIntegrationEventHandler<NotificationIntegrationEvent>>(
                    notificationEvent.Topics
                );

            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation($"{nameof(NotificationBackgroundService)} task doing background work.");

                try
                {
                    var msg = _eventBus.Consume<NotificationIntegrationEvent>();
                    SendEmailCommand(msg, logger, mediator, stoppingToken);

                    await Task.Delay(milisecondsDelay, stoppingToken);
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"{nameof(NotificationBackgroundService)} exception:{e.Message}");
                }

                logger.LogInformation($"{nameof(NotificationBackgroundService)} background task is stopping.");
            }
        }

        private void SendEmailCommand(NotificationIntegrationEvent msg,
            ILogger<NotificationBackgroundService> logger,
            ISender mediator, CancellationToken stoppingToken)
        {
            logger.LogInformation($"{nameof(SendEmailCommand)} for:{JsonConvert.SerializeObject(msg)}.");

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
            mediator.Send(command, stoppingToken);
        }
    }

    public class  BackgroundServiceOptions
    {
        public int MilisecondsDelay { get; set; } = 0;
    }
}
