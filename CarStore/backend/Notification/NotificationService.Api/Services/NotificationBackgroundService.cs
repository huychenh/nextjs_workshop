using Confluent.Kafka;
using N8T.Infrastructure.Bus;
using N8T.Infrastructure.Validator;
using Newtonsoft.Json;
using NotificationService.Shared.Events;
using SendGrid;
using SendGrid.Helpers.Mail;

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
            using var eventBus = provider.GetRequiredService<IEventBus>();
            var sendGridClient = provider.GetRequiredService<ISendGridClient>(); 

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

                    var email = new SendGridMessage()
                    {
                        From = new EmailAddress(msg.From, msg.FromName),
                        Subject = msg.Subject,
                        HtmlContent = msg.Body,
                    };
                    email.AddTo(new EmailAddress(msg.To));

                    var response = await sendGridClient.SendEmailAsync(email, stoppingToken);
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
