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
{   //TODO: how to store offset from last proceed
    public class EventBusKafka : IEventBus, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly IConsumer<Ignore, string> _consumer;
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

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _kafkaOptions.BootstrapServers,
                GroupId = _kafkaOptions.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }

        public async Task PublishAsync<TEvent>(TEvent @event, string[] topics, CancellationToken cancellationToken = default)
            where TEvent : IDomainEvent
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            if (topics == null)
            {
                throw new ArgumentNullException(nameof(topics));
            }

            var messageValue = JsonConvert.SerializeObject(@event);

            foreach (var topic in topics)
            {
                _logger.LogInformation("Publishing event {@Event} to {TopicName}", @event, topic);
                await _producer.ProduceAsync(topic, new Message<Null, string> { Value = messageValue }, cancellationToken);
            }
        }

        public void Subscribe<TEvent, THandler>(string[] topics)
            where TEvent : IDomainEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            if (topics == null)
            {
                throw new ArgumentNullException(nameof(topics));
            }

            _logger.LogInformation("Subscribe to {Topics}", topics);
            _consumer.Subscribe(topics);
        }

        public TEvent Consume<TEvent>(CancellationToken cancellationToken = default)
            where TEvent : IDomainEvent
        {
            var result = _consumer.Consume(cancellationToken);
            return JsonConvert.DeserializeObject<TEvent>(result.Message.Value);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _producer.Dispose();
                    _consumer.Close();
                    _consumer.Dispose();
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
