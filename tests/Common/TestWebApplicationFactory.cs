using datntdev.Microservice.Shared.Common;
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

            // Replace ISrvIdentityHttpClient with a mock that builds a SessionDto from request headers
            services.AddScoped<ISrvIdentityHttpClient>(sp =>
            {
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var mock = Substitute.For<ISrvIdentityHttpClient>();

                mock.Identities_GetSessionAsync()
                    .Returns(_ =>
                    {
                        var request = httpContextAccessor.HttpContext?.Request;
                        var sub = request?.Headers["X-Test-Sub"].ToString() ?? "1";
                        var permsHeader = request?.Headers["X-Test-Permissions"].ToString() ?? "";

                        var permissions = permsHeader
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => Enum.Parse<Constants.Permissions>(p.Trim(), ignoreCase: true))
                            .ToArray();

                        return Task.FromResult(new SessionDto
                        {
                            User = new SessionUserDto
                            {
                                Id = long.TryParse(sub, out var id) ? id : 1,
                                EmailAddress = "test@datntdev.com",
                                FirstName = "Test",
                                LastName = "User",
                                Permissions = permissions,
                                Roles = []
                            }
                        });
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
