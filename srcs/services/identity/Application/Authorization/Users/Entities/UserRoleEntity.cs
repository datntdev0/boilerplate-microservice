using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Roles.Entities;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Users.Entities;

public class UserRoleEntity : ICreated
{
    public long UserId { get; set; }
    public int RoleId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }

    public UserEntity User { get; set; } = default!;
    public RoleEntity Role { get; set; } = default!;
}