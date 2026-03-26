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

            return services;
        }
    }
}
