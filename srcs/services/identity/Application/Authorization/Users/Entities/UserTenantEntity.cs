using datntdev.Microservice.Shared.Common.Model;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Users.Entities;

public class UserTenantEntity : ICreated
{
    public long UserId { get; set; }
    public long TenantId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }

    public UserEntity User { get; set; } = default!;
}