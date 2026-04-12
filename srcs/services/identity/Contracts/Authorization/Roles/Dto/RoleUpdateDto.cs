using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Model;

namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Roles.Dto;

public class RoleUpdateDto : BaseDto<int>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Constants.Permissions[] Permissions { get; set; } = [];
}
