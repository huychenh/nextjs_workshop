using N8T.Core.Domain;
using System.Threading.Tasks;

namespace N8T.Infrastructure.Bus
{
    public interface IIntegrationEventHandler<in TIntegrationEvent>
        where TIntegrationEvent : IDomainEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}
