using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Roles.Dto;

namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;

public class UserDto : BaseAuditDto<long>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public RoleDto[] Roles { get; set; } = [];
    public Constants.Permissions[] Permissions { get; set; } = [];
}
