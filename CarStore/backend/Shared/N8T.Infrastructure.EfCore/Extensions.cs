using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using N8T.Core.Domain;
using N8T.Infrastructure.EfCore.Internal;

namespace N8T.Infrastructure.EfCore
{
    public static class Extensions
    {
        public static IServiceCollection AddPostgresDbContext<TDbContext>(
            this IServiceCollection services,
            string connString, 
            Action<DbContextOptionsBuilder> doMoreDbContextOptionsConfigure = null,
            Action<IServiceCollection> doMoreActions = null)
            where TDbContext : DbContext, IDbFacadeResolver, IDomainEventContext
        {
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseNpgsql(connString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(TDbContext).Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                }).UseSnakeCaseNamingConvention();

                doMoreDbContextOptionsConfigure?.Invoke(options);
            });

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<TDbContext>());
            
            services.AddScoped<IDomainEventContext>(provider => provider.GetService<TDbContext>());

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TxBehavior<,>));

            services.AddHostedService<DbContextMigratorHostedService>();

            doMoreActions?.Invoke(services);

            return services;
        }
    }
}
