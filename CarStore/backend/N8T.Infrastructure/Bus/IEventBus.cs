using System.Threading;
using System.Threading.Tasks;
using N8T.Core.Domain;

namespace N8T.Infrastructure.Bus
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent @event, string[] topics, CancellationToken token = default)
            where TEvent : IDomainEvent;

        void Subscribe<TEvent, THandler>(string[] topics)
            where TEvent : IDomainEvent
            where THandler : IIntegrationEventHandler<TEvent>;

        TEvent Consume<TEvent>(CancellationToken cancellationToken = default)
            where TEvent : IDomainEvent;
    }
}
