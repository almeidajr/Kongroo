using Kongroo.BuildingBlocks;
using Kongroo.BuildingBlocks.Application;
using Kongroo.BuildingBlocks.Contracts;
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
            services.AddScoped<
                IIntegrationEventHandler<OrderCompletedIntegrationEvent>,
                OrderCompletedIntegrationEventHandler
            >();

            return services;
        }

        private IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services.AddOutboxDbContext<LibraryDbContext>(configuration);

            return services;
        }
    }
}
