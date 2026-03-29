using datntdev.Microservice.Shared.Common.Model;

namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;

public class UserUpdateDto : BaseDto<long>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
