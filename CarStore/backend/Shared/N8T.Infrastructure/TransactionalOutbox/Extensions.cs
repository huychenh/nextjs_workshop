using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace N8T.Infrastructure.TransactionalOutbox
{
    public static class Extensions
    {
        public static IServiceCollection AddTransactionalOutbox(this IServiceCollection services, IConfiguration config, string provider = "dapr")
        {
            switch (provider)
            {
                //case "dapr":
                //{
                //    services.Configure<DaprTransactionalOutboxOptions>(config.GetSection(DaprTransactionalOutboxOptions.Name));
                //    services.AddScoped<INotificationHandler<EventWrapper>, LocalDispatchedHandler>();
                //    services.AddScoped<ITransactionalOutboxProcessor, TransactionalOutboxProcessor>();
                //    break;
                //}
            }

            return services;
        }
    }
}
