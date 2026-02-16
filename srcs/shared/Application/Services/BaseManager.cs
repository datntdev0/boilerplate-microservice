using datntdev.Microservice.Shared.Application.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace datntdev.Microservice.Shared.Application.Services;

public abstract class BaseManager<TDbContext>
    where TDbContext : BaseDbContext
{
    protected readonly ILogger _logger;
    protected readonly TDbContext _dbContext;

    public BaseManager(IServiceProvider services)
    {
        _logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(GetType());
        _dbContext = services.GetRequiredService<TDbContext>();
    }
}
