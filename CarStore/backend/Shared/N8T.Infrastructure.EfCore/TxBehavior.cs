using System;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using N8T.Core.Domain;
using N8T.Infrastructure.Bus;

namespace N8T.Infrastructure.EfCore
{
    //[DebuggerStepThrough]
    public class TxBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly IDomainEventContext _domainEventContext;
        private readonly IDbFacadeResolver _dbFacadeResolver;
        private readonly IMediator _mediator;
        private readonly ILogger<TxBehavior<TRequest, TResponse>> _logger;
        private readonly IEventBus _eventBus;

        public TxBehavior(IDbFacadeResolver dbFacadeResolver, IDomainEventContext domainEventContext,
            IMediator mediator, ILogger<TxBehavior<TRequest, TResponse>> logger, IEventBus eventBus)
        {
            _domainEventContext = domainEventContext ?? throw new ArgumentNullException(nameof(domainEventContext));
            _dbFacadeResolver = dbFacadeResolver ?? throw new ArgumentNullException(nameof(dbFacadeResolver));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (request is not ITxRequest)
            {
                return await next();
            }

            _logger.LogInformation("{Prefix} Handled command {MediatRRequest}", nameof(TxBehavior<TRequest, TResponse>), typeof(TRequest).FullName);
            _logger.LogDebug("{Prefix} Handled command {MediatRRequest} with content {RequestContent}", nameof(TxBehavior<TRequest, TResponse>), typeof(TRequest).FullName, JsonSerializer.Serialize(request));
            _logger.LogInformation("{Prefix} Open the transaction for {MediatRRequest}", nameof(TxBehavior<TRequest, TResponse>), typeof(TRequest).FullName);
            var strategy = _dbFacadeResolver.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                // Achieving atomicity
                await using var transaction = await _dbFacadeResolver.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

                var response = await next();
                _logger.LogInformation("{Prefix} Executed the {MediatRRequest} request", nameof(TxBehavior<TRequest, TResponse>), typeof(TRequest).FullName);

                await transaction.CommitAsync(cancellationToken);

                var domainEvents = _domainEventContext.GetDomainEvents().ToList();
                _logger.LogInformation("{Prefix} Published domain events for {MediatRRequest}", nameof(TxBehavior<TRequest, TResponse>), typeof(TRequest).FullName);

                var tasks = domainEvents.Select(async @event =>
                {
                    await _eventBus.PublishAsync(@event, @event.Topics, cancellationToken);
                });

                // Hack: remove 'await' to return immediately (in case we have not setup Kafka yet)
                Task.WhenAll(tasks);

                return response;
            });
        }
    }
}
