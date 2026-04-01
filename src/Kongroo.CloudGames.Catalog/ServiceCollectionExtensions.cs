using Kongroo.CloudGames.Catalog.Application;
using Kongroo.CloudGames.Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Kongroo.CloudGames.Catalog;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCatalogModule(IConfiguration configuration)
        {
            services.AddValidation();
            services.AddApplication();
            services.AddInfrastructure(configuration);

            return services;
        }

        private IServiceCollection AddApplication()
        {
            services.AddScoped<CreatePromotionCommandHandler>();
            services.AddScoped<CreateGameCommandHandler>();
            services.AddScoped<GetGameQueryHandler>();
            services.AddScoped<GetOrderQueryHandler>();
            services.AddScoped<GetOrdersQueryHandler>();
            services.AddScoped<GetGamesQueryHandler>();
            services.AddScoped<PlaceOrderCommandHandler>();
            services.AddScoped<UpdateGameCommandHandler>();
            services.AddScoped<DeleteGameCommandHandler>();

            return services;
        }

        private IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services.TryAddSingleton(TimeProvider.System);

            services.AddSingleton<OutboxMessagesInterceptor>();

            services.AddDbContext<CatalogDbContext>(
                (serviceProvider, contextOptions) =>
                    contextOptions
                        .EnableDetailedErrors()
                        .EnableSensitiveDataLogging()
                        .AddInterceptors(serviceProvider.GetRequiredService<OutboxMessagesInterceptor>())
                        .UseNpgsql(
                            configuration.GetConnectionString("Database"),
                            postgresOptions =>
                                postgresOptions.MigrationsHistoryTable("migrations", CatalogDbContext.Schema)
                        )
                        .UseSnakeCaseNamingConvention()
            );

            return services;
        }
    }
}
