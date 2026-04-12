using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Model;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Roles.Entities;

public class RoleEntity : BaseAuditEntity<int>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Constants.Permissions[] Permissions { get; set; } = [];
}
