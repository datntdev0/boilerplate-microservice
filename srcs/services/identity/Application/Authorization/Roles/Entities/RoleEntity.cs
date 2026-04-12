using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Users.Entities;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Roles.Entities;

public class RoleEntity : BaseAuditEntity<int>, ITenancy
{
    public int? TenantId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Constants.Permissions[] Permissions { get; set; } = [];

    public List<UserEntity> Users { get; set; } = [];
}
