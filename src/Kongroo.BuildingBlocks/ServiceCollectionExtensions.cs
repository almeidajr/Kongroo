using Kongroo.BuildingBlocks.Application;
using Kongroo.BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kongroo.BuildingBlocks;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddBuildingBlocks(IConfiguration configuration)
        {
            services.AddSingleton(TimeProvider.System);
            services.AddSingleton<OutboxMessagesInterceptor>();
            services.AddScoped<IEventBus, InProcessEventBus>();

            services
                .AddOptions<OutboxProcessingOptions>()
                .Bind(configuration.GetRequiredSection(OutboxProcessingOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }

        public IServiceCollection AddOutboxContext<TDbContext>(IConfiguration configuration)
            where TDbContext : OutboxDbContext<TDbContext>, IRelationalDbContext
        {
            services.AddDbContext<TDbContext>(
                (serviceProvider, contextOptions) =>
                    contextOptions
                        .EnableDetailedErrors()
                        .EnableSensitiveDataLogging()
                        .AddInterceptors(serviceProvider.GetRequiredService<OutboxMessagesInterceptor>())
                        .UseNpgsql(
                            configuration.GetConnectionString("Database"),
                            postgresOptions => postgresOptions.MigrationsHistoryTable("migrations", TDbContext.Schema)
                        )
                        .UseSnakeCaseNamingConvention()
            );
            services.AddScoped<OutboxMessageProcessor<TDbContext>>();
            services.AddHostedService<OutboxMessageProcessorHostedService<TDbContext>>();

            return services;
        }
    }
}
