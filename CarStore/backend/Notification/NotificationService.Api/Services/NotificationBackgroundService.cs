using CarStore.AppContracts.Dtos;
using CarStore.IntegrationEvents.Notification;
using Confluent.Kafka;
using MediatR;
using N8T.Infrastructure.Bus;
using N8T.Infrastructure.Validator;
using Newtonsoft.Json;
using NotificationService.AppCore.UseCases.Commands;

namespace NotificationService.Api.Services
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly ILogger<NotificationBackgroundService> _logger;

        private readonly IServiceProvider _serviceProvider;

        public NotificationBackgroundService(
              ILogger<NotificationBackgroundService> logger
            , IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() =>
                _logger.LogInformation($"{nameof(NotificationBackgroundService)} is cancelled."));

            _logger.LogInformation($"{nameof(NotificationBackgroundService)} is started.");

            new Thread(async () => await ConsumeNotificationIntegrationEvent(_serviceProvider, stoppingToken)).Start();

            return Task.CompletedTask;
        }

        private async Task ConsumeNotificationIntegrationEvent(IServiceProvider provider, CancellationToken stoppingToken)
        {
            using var scope = provider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<NotificationBackgroundService>>();
            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();
            using var eventBus = provider.GetRequiredService<IEventBus>();

            var @event = new NotificationIntegrationEvent();
            eventBus
                .Subscribe<NotificationIntegrationEvent, IIntegrationEventHandler<NotificationIntegrationEvent>>(
                    @event.Topics
                );

            logger.LogInformation($"{nameof(NotificationBackgroundService)} subscribe " +
                $"topics: {string.Join(',', @event.Topics)} completed.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var msg = eventBus.Consume<NotificationIntegrationEvent>(stoppingToken);                    
                    if (msg == null)
                    {
                        continue;
                    }

                    logger.LogInformation($"{nameof(NotificationBackgroundService)}: {JsonConvert.SerializeObject(msg)}.");

                    await mediator.Send(new SendNotification.Command
                    {
                        Model = new NotificationDto
                        {
                            From = msg.From,
                            FromName = msg.FromName,
                            To = msg.To,
                            Subject = msg.Subject,
                            Body = msg.Body,
                        }
                    }, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ValidationException ex)
                {
                    logger.LogError(ex, $"{nameof(NotificationBackgroundService)}:{ex.ValidationResultModel}");
                }
                catch (ConsumeException ex)
                {
                    logger.LogError(ex, $"{nameof(NotificationBackgroundService)}:{ex.Message}");
                    //Topic not created yet
                    if (ex.Error.Code == ErrorCode.UnknownTopicOrPart)
                    {
                        await eventBus.PublishAsync(@event, @event.Topics, stoppingToken);
                        logger.LogInformation($"{string.Join(',', @event.Topics)} created");
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"{nameof(NotificationBackgroundService)} exception:{ex.Message}");
                    throw;
                }
            }
            logger.LogInformation($"{nameof(NotificationBackgroundService)} background task is stopped.");
        }
    }
}
