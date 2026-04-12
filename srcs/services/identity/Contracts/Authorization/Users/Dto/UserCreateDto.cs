using datntdev.Microservice.Shared.Common;

namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;

public class UserCreateDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public int[] RoleIds { get; set; } = [];
    public Constants.Permissions[] Permissions { get; set; } = [];
}
