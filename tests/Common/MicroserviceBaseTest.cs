using Aspire.Hosting.Testing;

namespace datntdev.Microservice.Tests.Common;

public abstract class MicroserviceBaseTest<TAspireApp>
{
    public static AspireTestingAppHost<TAspireApp> AppHost { get; } = new();

    public TestContext TestContext { get; set; } = default!;

    public CancellationToken CancellationToken => TestContext.CancellationTokenSource.Token;

    public abstract HttpClient HttpClient { get; }

    public static Task StaticAssemblyInitialize(TestContext testContext)
    {
        Console.WriteLine("Starting Aspire Testing App Host...");
        var token = testContext.CancellationTokenSource.Token;
        return AppHost.StartAsync().WaitAsync(TimeSpan.FromSeconds(60), token);
    }
}

public class AspireTestingAppHost<TAspireApp>() : DistributedApplicationFactory(typeof(TAspireApp))
{
}