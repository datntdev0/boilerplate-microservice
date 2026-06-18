using datntdev.Microservice.Shared.Common.Helpers;
using datntdev.Microservice.Shared.Communication.Extensions;
using datntdev.Microservice.Shared.Communication.HttpClients;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;
using datntdev.Microservice.Tests.Common.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace datntdev.Microservice.Tests.Common;

public class TestWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
    where TEntryPoint : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddHttpContextAccessor();

            // Replace OpenIddict validation with a lightweight test auth scheme
            services.AddAuthentication(TestAuthHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });

            // Replace all ISrv*HttpClient with keyed mocks (both default and apikey variants)
            ISrvIdentityHttpClient CreateIdentityMock(IServiceProvider sp)
            {
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var mock = Substitute.For<ISrvIdentityHttpClient>();

                mock.Identities_GetSessionAsync()
                    .Returns(_ =>
                    {
                        var request = httpContextAccessor.HttpContext?.Request;
                        var authHeader = request?.Headers.Authorization.ToString();
                        var base64 = StringHelper.GetSubstring(authHeader, "TestKey");
                        if (string.IsNullOrEmpty(base64)) return Task.FromResult(new SessionDto());

                        try
                        {
                            var json = StringHelper.ConvertFromBase64(base64);
                            var sessionDto = JsonHelper.Deserialize<SessionDto>(json);
                            return Task.FromResult(sessionDto ?? new SessionDto());
                        }
                        catch
                        {
                            return Task.FromResult(new SessionDto());
                        }
                    });

                return mock;
            }

            services.AddKeyedScoped<ISrvIdentityHttpClient>(HttpProxyServiceTypes.TypeDefault, (sp, _) => CreateIdentityMock(sp));
            services.AddKeyedScoped<ISrvIdentityHttpClient>(HttpProxyServiceTypes.TypeApiKey, (sp, _) => CreateIdentityMock(sp));
            services.AddKeyedScoped<ISrvAdminHttpClient>(HttpProxyServiceTypes.TypeDefault, (_, _) => Substitute.For<ISrvAdminHttpClient>());
            services.AddKeyedScoped<ISrvAdminHttpClient>(HttpProxyServiceTypes.TypeApiKey, (_, _) => Substitute.For<ISrvAdminHttpClient>());
            services.AddKeyedScoped<ISrvNotifyHttpClient>(HttpProxyServiceTypes.TypeDefault, (_, _) => Substitute.For<ISrvNotifyHttpClient>());
            services.AddKeyedScoped<ISrvNotifyHttpClient>(HttpProxyServiceTypes.TypeApiKey, (_, _) => Substitute.For<ISrvNotifyHttpClient>());
            services.AddKeyedScoped<ISrvPaymentHttpClient>(HttpProxyServiceTypes.TypeDefault, (_, _) => Substitute.For<ISrvPaymentHttpClient>());
            services.AddKeyedScoped<ISrvPaymentHttpClient>(HttpProxyServiceTypes.TypeApiKey, (_, _) => Substitute.For<ISrvPaymentHttpClient>());
        });
    }
}
