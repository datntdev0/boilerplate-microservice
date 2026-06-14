using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Helpers;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;

namespace datntdev.Microservice.Tests.Common;

public abstract class MicroserviceBaseTest<TEntryPoint> where TEntryPoint : class
{
    public static TestWebApplicationFactory<TEntryPoint> AppFactory { get; } = new();

    public TestContext TestContext { get; set; } = default!;

    public CancellationToken CancellationToken => TestContext.CancellationTokenSource.Token;

    public abstract HttpClient HttpClient { get; }

    private static readonly Constants.Permissions[] AllPermissions =
    [
        Constants.Permissions.Tenancy,
        Constants.Permissions.Tenancy_Read,
        Constants.Permissions.Tenancy_Write,
        Constants.Permissions.Users,
        Constants.Permissions.Users_Read,
        Constants.Permissions.Users_Write,
        Constants.Permissions.Roles,
        Constants.Permissions.Roles_Read,
        Constants.Permissions.Roles_Write,
    ];

    /// <summary>
    /// Creates an authenticated HTTP client using a SessionDto.
    /// Serializes the SessionDto to JSON, encodes it as base64, and sets it in the Authorization header.
    /// </summary>
    public HttpClient CreateAuthenticatedClient(SessionDto sessionDto)
    {
        var client = AppFactory.CreateClient();
        var token = StringHelper.ConvertToBase64(JsonHelper.Serialize(sessionDto));
        client.DefaultRequestHeaders.Add("Authorization", $"TestKey {token}");
        return client;
    }

    /// <summary>
    /// Creates an authenticated HTTP client with all permissions for general-purpose test use.
    /// </summary>
    public Task<HttpClient> GetAuthenticatedClientAsync()
    {
        var sessionDto = new SessionDto
        {
            User = new SessionUserDto
            {
                Id = 1,
                EmailAddress = "admin@datntdev.com",
                FirstName = "Admin",
                LastName = "User",
                Permissions = AllPermissions,
                Roles = []
            }
        };
        return Task.FromResult(CreateAuthenticatedClient(sessionDto));
    }

    /// <summary>
    /// Creates an authenticated HTTP client with no permissions so auth middleware reaches the
    /// permission-check step and returns 403 (not 401 from an unauthenticated caller).
    /// </summary>
    public HttpClient CreateClientWithoutPermissions()
    {
        var sessionDto = new SessionDto
        {
            User = new SessionUserDto
            {
                Id = 99,
                EmailAddress = "noperm@example.com",
                FirstName = "No",
                LastName = "Permission",
                Permissions = [],
                Roles = []
            }
        };
        return CreateAuthenticatedClient(sessionDto);
    }

    public static Task StaticAssemblyInitialize(TestContext testContext)
    {
        Console.WriteLine("Initializing WebApplicationFactory for integration tests...");
        return Task.CompletedTask;
    }
}