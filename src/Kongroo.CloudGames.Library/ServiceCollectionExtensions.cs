using Kongroo.CloudGames.Library.Application;
using Kongroo.CloudGames.Library.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kongroo.CloudGames.Library;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddLibraryModule(IConfiguration configuration)
        {
            services.AddValidation();
            services.AddApplication();
            services.AddInfrastructure(configuration);

            return services;
        }

        private IServiceCollection AddApplication()
        {
            services.AddScoped<AcquireGameOwnershipCommandHandler>();
            services.AddScoped<GetGameOwnershipQueryHandler>();
            services.AddScoped<GetGameOwnershipsQueryHandler>();

            return services;
        }

        private IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services.AddDbContext<LibraryDbContext>(contextOptions =>
                contextOptions
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .UseNpgsql(
                        configuration.GetConnectionString("Database"),
                        postgresOptions => postgresOptions.MigrationsHistoryTable("migrations", LibraryDbContext.Schema)
                    )
                    .UseSnakeCaseNamingConvention()
            );

            return services;
        }
    }
}
