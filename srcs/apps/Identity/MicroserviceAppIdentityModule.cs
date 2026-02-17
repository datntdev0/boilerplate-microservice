using datntdev.Microservice.App.Identity.Identity;
using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Modular;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace datntdev.Microservice.App.Identity;

public class MicroserviceAppIdentityModule : BaseModule
{
    public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddDbContext<MicroserviceAppIdentityDbContext>(opt 
            => opt.UseSqlServer(configs.GetConnectionString("App.Identity")));
        services.AddOpenIddictServices(configs);
    }
}

internal static class MicroserviceAppIdentityModuleExtensions
{
    public static void AddIdentityServices(this IServiceCollection services)
    {
        services.AddAuthentication(Constants.Application.AuthenticationScheme)
                .AddCookie(Constants.Application.AuthenticationScheme, options =>
                {
                    options.LoginPath = Constants.Endpoints.AuthSignIn;
                    options.LogoutPath = Constants.Endpoints.AuthSignOut;
                });

        services.AddAuthorization()
                .AddAuthenticationCore()
                .AddCascadingAuthenticationState();

        services.AddScoped<IdentityManager>()
                .AddSingleton<PasswordHasher>()
                .AddHttpContextAccessor();
    }

    public static IServiceCollection AddOpenIddictServices(
            this IServiceCollection services, IConfigurationRoot configs)
    {
        services.ConfigureDbContext<MicroserviceAppIdentityDbContext>(opt => opt.UseOpenIddict());

        var encryptionKey = Convert.FromBase64String(configs["OpenIddict:EncryptionKey"]!);

        services.AddOpenIddict()
            .AddCore(opt => opt.UseEntityFrameworkCore().UseDbContext<MicroserviceAppIdentityDbContext>())
            .AddServer(options =>
            {
                // Enable this if you want to encrypt access tokens
                options.DisableAccessTokenEncryption();

                options
                    .AddEphemeralSigningKey()
                    .AddEncryptionKey(new SymmetricSecurityKey(encryptionKey));

                options
                    .RequireProofKeyForCodeExchange()
                    .AllowAuthorizationCodeFlow()
                    .AllowClientCredentialsFlow();

                options
                    .SetTokenEndpointUris(Constants.Endpoints.OAuth2Token)
                    .SetAuthorizationEndpointUris(Constants.Endpoints.OAuth2Authorize)
                    .SetEndSessionEndpointUris(Constants.Endpoints.AuthSignOut);

                options.UseAspNetCore()
                    .EnableTokenEndpointPassthrough()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableEndSessionEndpointPassthrough();
            });
        return services;
    }

}