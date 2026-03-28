using datntdev.Microservice.Shared.Common.Model;

namespace datntdev.Microservice.Srv.Admin.Application.Tenancy.Entities;

public class TenantEntity : FullAuditEntity<int>
{
    public string Name { get; set; } = default!;
    public string Organization { get; set; } = default!;
}
