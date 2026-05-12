using datntdev.Microservice.Shared.Common;

namespace datntdev.Microservice.Tests.Common;

public abstract class MicroserviceBaseTest<TEntryPoint>
    where TEntryPoint : class
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
    /// Creates an isolated test client authenticated with the specified user ID and permissions.
    /// Uses test-only headers (X-Test-Sub, X-Test-Permissions) — no live Identity service required.
    /// </summary>
    public HttpClient CreateAuthenticatedClient(string userId, params Constants.Permissions[] permissions)
    {
        var client = AppFactory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Test-Sub", userId);
        if (permissions.Length > 0)
            client.DefaultRequestHeaders.Add("X-Test-Permissions", string.Join(",", permissions.Select(p => p.ToString())));
        return client;
    }

    /// <summary>
    /// Creates an authenticated HTTP client with all permissions for general-purpose test use.
    /// </summary>
    public Task<HttpClient> GetAuthenticatedClientAsync()
        => Task.FromResult(CreateAuthenticatedClient("1", AllPermissions));

    public static Task StaticAssemblyInitialize(TestContext testContext)
    {
        Console.WriteLine("Initializing WebApplicationFactory for integration tests...");
        return Task.CompletedTask;
    }
}