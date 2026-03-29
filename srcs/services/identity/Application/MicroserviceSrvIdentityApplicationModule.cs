using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Roles;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Users;
using datntdev.Microservice.Srv.Identity.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Identity.Application;

[DependOn(typeof(MicroserviceSrvIdentityContractModule))]
public class MicroserviceSrvIdentityApplicationModule : BaseModule
{
    override public void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddDbContext<MicroserviceSrvIdentityDbContext>();

        services.AddScoped<UsersManager>();
        services.AddScoped<UserCreatingValidator>();
        services.AddScoped<UserUpdatingValidator>();

        services.AddScoped<RolesManager>();
        services.AddScoped<RoleCreatingValidator>();
        services.AddScoped<RoleUpdatingValidator>();
    }
}
