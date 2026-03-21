using Microsoft.Extensions.DependencyInjection;

namespace Kongroo.CloudGames.Identity;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddIdentityModule()
        {
            services.AddValidation();
            return services;
        }
    }
}
