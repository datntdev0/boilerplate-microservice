using datntdev.Microservice.Shared.Common.Model;

namespace datntdev.Microservice.Srv.Admin.Contracts.Tenancy.Dto;

public class TenantListDto : BaseAuditDto<int>
{
    public string Name { get; set; } = default!;
    public string Organization { get; set; } = default!;
}
