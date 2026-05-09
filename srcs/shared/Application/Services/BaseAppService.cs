using datntdev.Microservice.Shared.Application.Authorization;
using datntdev.Microservice.Shared.Common.Application;
using datntdev.Microservice.Shared.Common.Model;
using Mapster;

namespace datntdev.Microservice.Shared.Application.Services;

public abstract class BaseAppService : IAppService
{
    static BaseAppService()
    {
        TypeAdapterConfig.GlobalSettings
            .When((srcType, destType, _) => destType.IsAssignableTo(typeof(BaseEntity)))
            .Ignore(
                nameof(BaseEntity<int>.Id),
                nameof(BaseAuditEntity<int>.ConcurrencyStamp),
                nameof(BaseAuditEntity<int>.CreatedAt),
                nameof(BaseAuditEntity<int>.CreatedBy),
                nameof(BaseAuditEntity<int>.UpdatedAt),
                nameof(BaseAuditEntity<int>.UpdatedBy),
                nameof(FullAuditEntity<int>.IsDeleted));
    }

    /// <summary>
    /// Provides access to session information (app metadata and authenticated user data).
    /// Automatically injected by PropertyInjectionFilter before action execution.
    /// </summary>
    [AppInject]
    protected SessionAppProvider SessionProvider { get; set; } = default!;

    protected static TDestination Map<TDestination>(object source)
        => source.Adapt<TDestination>();

    protected static void MapTo<TSource, TDestination>(TSource source, TDestination destination)
        => source.Adapt(destination);
}
