using Kongroo.BuildingBlocks;
using Kongroo.CloudGames.Identity.Application;
using Kongroo.CloudGames.Identity.Application.Abstractions;
using Kongroo.CloudGames.Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;
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
            services.AddScoped<AuthenticateUserCommandHandler>();
            services.AddScoped<CreateUserCommandHandler>();
            services.AddScoped<GetUserQueryHandler>();
            services.AddScoped<GetUsersQueryHandler>();
            services.AddScoped<UpdateUserRoleCommandHandler>();

            return services;
        }

        private IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services.AddOutboxContext<IdentityDbContext>(configuration);

            services
                .AddOptions<JwtOptions>()
                .Bind(configuration.GetRequiredSection(JwtOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddSingleton<IAccessTokenIssuer, JwtAccessTokenIssuer>();
            services.AddSingleton<IPasswordHasher<string>, PasswordHasher<string>>();

            return services;
        }
    }
}
