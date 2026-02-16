using datntdev.Microservice.App.Identity.Identity;
using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Modular;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.App.Identity;

public class MicroserviceAppIdentityModule : BaseModule
{
    public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddDbContext<MicroserviceAppIdentityDbContext>(opt
            => opt.UseSqlServer(configs.GetConnectionString("App.Identity")));
        services.AddIdentityServices();
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

        services.AddScoped<IdentityManager>()
                .AddSingleton<PasswordHasher>()
                .AddHttpContextAccessor();
    }
}