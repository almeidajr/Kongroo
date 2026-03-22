using Kongroo.CloudGames.Identity.Application;
using Kongroo.CloudGames.Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kongroo.CloudGames.Identity;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddIdentityModule(IConfiguration configuration)
        {
            services.AddValidation();
            services.AddApplication();
            services.AddInfrastructure(configuration);
            return services;
        }

        private IServiceCollection AddApplication()
        {
            services.AddScoped<CreateUserCommandHandler>();
            services.AddSingleton<IPasswordHasher<string>, PasswordHasher<string>>();
            return services;
        }

        private IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services.AddDbContext<IdentityDbContext>(contextOptions =>
                contextOptions
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .UseNpgsql(
                        configuration.GetConnectionString("Database"),
                        postgresOptions =>
                            postgresOptions.MigrationsHistoryTable("migrations", IdentityDbContext.Schema)
                    )
                    .UseSnakeCaseNamingConvention()
            );

            return services;
        }
    }
}
