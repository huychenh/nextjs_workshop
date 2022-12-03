using Confluent.Kafka;
using MediatR;
using N8T.Infrastructure.Bus;
using N8T.Infrastructure.Validator;
using Newtonsoft.Json;
using NotificationService.Shared.DTO;
using OrderingService.AppCore.UseCases.Commands;
using OrderingService.Shared.Events;

namespace OrderingService.Api.Services
{
    public class OrderCreatedBackgroundService : BackgroundService
    {
        private readonly ILogger<OrderCreatedBackgroundService> _logger;

        private readonly IServiceProvider _serviceProvider;

        public OrderCreatedBackgroundService(
              ILogger<OrderCreatedBackgroundService> logger
            , IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() =>
                _logger.LogInformation($"{nameof(OrderCreatedBackgroundService)} is cancelled."));

            _logger.LogInformation($"{nameof(OrderCreatedBackgroundService)} is started.");

            new Thread(async () => await ConsumeOrderCreatedEvent(_serviceProvider, stoppingToken)).Start();

            return Task.CompletedTask;
        }

        private async Task ConsumeOrderCreatedEvent(IServiceProvider provider, CancellationToken stoppingToken)
        {
            using var scope = provider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderCreatedBackgroundService>>();
            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();
            using var eventBus = provider.GetRequiredService<IEventBus>();

            var @event = new OrderCreatedIntegrationEvent();
            eventBus
                .Subscribe<OrderCreatedIntegrationEvent, IIntegrationEventHandler<OrderCreatedIntegrationEvent>>(
                    @event.Topics
                );

            logger.LogInformation($"{nameof(OrderCreatedBackgroundService)} subscribe " +
                $"topics: {string.Join(',', @event.Topics)} completed.");

            logger.LogInformation($"Start {nameof(ConsumeOrderCreatedEvent)}.");


            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var msg = eventBus.Consume<OrderCreatedIntegrationEvent>(stoppingToken);
                    if (msg == null)
                    {
                        continue;
                    }

                    logger.LogInformation($"{nameof(OrderCreatedIntegrationEvent)}: {JsonConvert.SerializeObject(msg)}.");

                    await mediator.Send(new NotifyOrderCreated.Command
                    {
                        Model = new NotifyOrderCreatedDto
                        {
                            BuyerEmail = msg.BuyerEmail,
                            OwnerEmail = msg.OwnerEmail,
                            OrderId = msg.OrderId,
                            ProductName = msg.ProductName,
                        }
                    }, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ValidationException ex)
                {
                    logger.LogError(ex, $"{nameof(OrderCreatedBackgroundService)}:{ex.ValidationResultModel}");
                }
                catch (ConsumeException ex)
                {
                    logger.LogError(ex, $"{nameof(OrderCreatedBackgroundService)}:{ex.Message}");
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
                    logger.LogError(ex, $"{nameof(OrderCreatedBackgroundService)}:{ex.Message}");
                    throw;
                }
            }
            logger.LogInformation($"{nameof(OrderCreatedBackgroundService)} is stopped.");
        }
    }
}