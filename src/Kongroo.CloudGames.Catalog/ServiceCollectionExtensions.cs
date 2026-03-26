using Kongroo.CloudGames.Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kongroo.CloudGames.Catalog;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCatalogModule(IConfiguration configuration)
        {
            services.AddValidation();
            services.AddInfrastructure(configuration);

            return services;
        }

        private IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services.AddDbContext<CatalogDbContext>(contextOptions =>
                contextOptions
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .UseNpgsql(
                        configuration.GetConnectionString("Database"),
                        postgresOptions => postgresOptions.MigrationsHistoryTable("migrations", CatalogDbContext.Schema)
                    )
                    .UseSnakeCaseNamingConvention()
            );

            return services;
        }
    }
}
