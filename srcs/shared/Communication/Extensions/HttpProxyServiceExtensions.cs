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

        services.AddScoped(typeof(SrvAdminHttpClient), (sp) =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            return new SrvAdminHttpClient(httpClientFactory.CreateClient("srv-admin"));
        });

        services.AddScoped(typeof(SrvNotifyHttpClient), (sp) =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            return new SrvNotifyHttpClient(httpClientFactory.CreateClient("srv-notify"));
        });

        services.AddScoped(typeof(SrvPaymentHttpClient), (sp) =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            return new SrvPaymentHttpClient(httpClientFactory.CreateClient("srv-payment"));
        });
    }
}
