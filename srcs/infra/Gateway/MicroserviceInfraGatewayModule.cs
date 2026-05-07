using datntdev.Microservice.Shared.Common.Modular;
using OpenIddict.Validation.AspNetCore;

namespace datntdev.Microservice.Infra.Gateway;

public class MicroserviceInfraGatewayModule : BaseModule
{
    public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        var authority = configs["OpenIddict:Authority"]!;

        services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        services.AddOpenIddict()
            .AddValidation(options =>
            {
                options.SetIssuer(authority);
                options.UseSystemNetHttp();
                options.UseAspNetCore();
            });
    }
}
