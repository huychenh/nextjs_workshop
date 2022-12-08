using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace N8T.Infrastructure.Bus
{
    public static class Extensions
    {
        public static IServiceCollection AddMessageBroker(this IMvcBuilder mvcBuilder, IConfiguration configuration)
        {
            mvcBuilder.Services.Configure<KafkaOptions>(configuration.GetSection("KafkaOptions"));
            mvcBuilder.Services.AddSingleton<IEventBus, EventBusKafka>();

            return mvcBuilder.Services;
        }
    }
}
