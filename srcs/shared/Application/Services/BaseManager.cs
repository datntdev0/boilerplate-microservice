using datntdev.Microservice.Shared.Application.Repository;
using datntdev.Microservice.Shared.Common.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace datntdev.Microservice.Shared.Application.Services;

public abstract class BaseManager { }

public abstract class BaseManager<TDbContext> : BaseManager
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

public abstract class BaseManager<TEntity, TDbContext>(IServiceProvider services) 
    : BaseManager<TDbContext>(services)
    where TEntity : BaseEntity
    where TDbContext : BaseDbContext
{
    public IQueryable<TEntity> Queryable => _dbContext.Set<TEntity>();
}

public abstract class BaseManager<TKey, TEntity, TDbContext>(IServiceProvider services) 
    : BaseManager<TEntity, TDbContext>(services)
    where TKey : IEquatable<TKey>
    where TEntity : BaseEntity<TKey>
    where TDbContext : BaseDbContext
{
    public abstract Task<TEntity> GetAsync(TKey id);
    public abstract Task<TEntity> CreateAsync(TEntity entity);
    public abstract Task<TEntity> UpdateAsync(TEntity entity);
    public abstract Task DeleteAsync(TKey id);
}