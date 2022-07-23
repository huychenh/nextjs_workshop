using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N8T.Core.Domain;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace N8T.Infrastructure.Bus
{
    public class EventBusKafka : IEventBus, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<EventBusKafka> _logger;
        private readonly KafkaOptions _kafkaOptions;
        private bool _disposedValue;

        public EventBusKafka(ILogger<EventBusKafka> logger, IOptions<KafkaOptions> kafkaOptionsAccessor)
        {
            _logger = logger;
            _kafkaOptions = kafkaOptionsAccessor.Value;
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _kafkaOptions.BootstrapServers,
                ClientId = Dns.GetHostName(),
            };
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public async Task PublishAsync<TEvent>(TEvent @event, string[] topics!!, CancellationToken cancellationToken = default)
            where TEvent : IDomainEvent
        {
            var messageValue = JsonConvert.SerializeObject(@event);

            foreach (var topic in topics)
            {
                _logger.LogInformation("Publishing event {@Event} to {TopicName}", @event, topic);
                await _producer.ProduceAsync(topic, new Message<Null, string> { Value = messageValue }, cancellationToken);
            }
        }

        public void Subscribe<TEvent, THandler>()
            where TEvent : IDomainEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _producer.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
