namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Tenants.Dto;

public class TenantUsersPatchDto
{
    public long[] Create { get; set; } = [];
    public long[] Delete { get; set; } = [];
}
