using CarStore.AppContracts.Dtos;
using CarStore.IntegrationEvents.Ordering;
using MediatR;
using Microsoft.Extensions.Options;
using N8T.Infrastructure.Bus;
using OrderingService.AppCore.UseCases.Commands;

namespace OrderingService.Api.Services
{
    public class OrderCreatedBackgroundService : BackgroundService
    {
        private readonly ILogger<OrderCreatedBackgroundService> _logger;

        private readonly IEventBus _eventBus;

        private readonly BackgroundServiceOptions _options;

        private readonly ISender _mediator;

        public OrderCreatedBackgroundService(IEventBus eventBus
            , ILogger<OrderCreatedBackgroundService> logger
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
                _logger.LogInformation($"{nameof(OrderCreatedBackgroundService)} background task is stopping."));

            _logger.LogInformation($"{nameof(OrderCreatedBackgroundService)} is starting.");

            _eventBus
                .Subscribe<OrderCreatedIntegrationEvent, IIntegrationEventHandler<OrderCreatedIntegrationEvent>>(
                    (new OrderCreatedIntegrationEvent()).Topics
                );

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(OrderCreatedBackgroundService)} task doing background work.");

                try
                {
                    ConsumeMessage(stoppingToken);

                    await Task.Delay(_options.MilisecondsDelay, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"{nameof(OrderCreatedBackgroundService)}:{e.Message}");
                }

                _logger.LogInformation($"{nameof(OrderCreatedBackgroundService)} background task is stopping.");
            }
        }

        private void ConsumeMessage(CancellationToken stoppingToken)
        {
            var msg = _eventBus.Consume<OrderCreatedIntegrationEvent>();

            _logger.LogInformation($"{nameof(ConsumeMessage)} for Order:{msg.OrderId}.");

            var command = new NotifyOrderCreated.Command
            {
                Model = new NotifyOrderCreatedDto
                {
                    BuyerEmail = msg.BuyerEmail,
                    OwnerEmail = msg.OwnerEmail,
                    OrderId = msg.OrderId,
                    ProductName = msg.ProductName,
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
