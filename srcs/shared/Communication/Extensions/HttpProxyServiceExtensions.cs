using datntdev.Microservice.Shared.Communication.HttpClients;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Shared.Communication.Extensions;

public static class HttpProxyServiceExtensions
{
    public static void AddHttpProxyService(this IServiceCollection services)
    {
        services.AddScoped(typeof(SrvIdentityHttpClient), (sp) =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            return new SrvIdentityHttpClient(httpClientFactory.CreateClient("srv-identity"));
        });
    }
}
