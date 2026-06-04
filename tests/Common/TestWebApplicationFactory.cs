using datntdev.Microservice.Shared.Common.Helpers;
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

            // Replace ISrvIdentityHttpClient with a mock that parses SessionDto from Authorization header
            services.AddScoped(sp =>
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
                            // Extract and decode SessionDto from Authorization header
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
            });

            // Replace remaining ISrv*HttpClient with no-op stubs
            services.AddScoped<ISrvAdminHttpClient>(_ => Substitute.For<ISrvAdminHttpClient>());
            services.AddScoped<ISrvNotifyHttpClient>(_ => Substitute.For<ISrvNotifyHttpClient>());
            services.AddScoped<ISrvPaymentHttpClient>(_ => Substitute.For<ISrvPaymentHttpClient>());
        });
    }
}
