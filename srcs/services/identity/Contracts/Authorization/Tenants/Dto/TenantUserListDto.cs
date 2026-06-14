namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Tenants.Dto;

public class TenantUserListDto
{
    public long UserId { get; set; }
    public string Email { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public DateTime? AssignedDate { get; set; }
}
