using Microsoft.AspNetCore.Mvc.Testing;

namespace datntdev.Microservice.Tests.Common;

public abstract class MicroserviceBaseTest<TEntryPoint>
    where TEntryPoint : class
{
    public static WebApplicationFactory<TEntryPoint> AppFactory { get; } = new();

    public TestContext TestContext { get; set; } = default!;

    public CancellationToken CancellationToken => TestContext.CancellationTokenSource.Token;

    public abstract HttpClient HttpClient { get; }

    public static Task StaticAssemblyInitialize(TestContext testContext)
    {
        Console.WriteLine("Initializing WebApplicationFactory for integration tests...");
        return Task.CompletedTask;
    }
}