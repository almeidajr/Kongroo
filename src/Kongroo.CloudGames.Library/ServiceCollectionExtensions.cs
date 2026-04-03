using Kongroo.BuildingBlocks;
using Kongroo.CloudGames.Library.Application;
using Kongroo.CloudGames.Library.Infrastructure;
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
            services.AddOutboxContext<LibraryDbContext>(configuration);

            return services;
        }
    }
}
