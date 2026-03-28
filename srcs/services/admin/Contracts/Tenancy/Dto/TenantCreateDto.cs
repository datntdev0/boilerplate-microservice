namespace datntdev.Microservice.Srv.Admin.Contracts.Tenancy.Dto;

public class TenantCreateDto
{
    public string Name { get; set; } = default!;
    public string Organization { get; set; } = default!;
}
