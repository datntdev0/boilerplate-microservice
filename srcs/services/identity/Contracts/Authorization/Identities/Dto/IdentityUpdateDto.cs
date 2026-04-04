using datntdev.Microservice.Shared.Common.Model;

namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;

public class IdentityUpdateDto : BaseDto<long>
{
    public string EmailAddress { get; set; } = default!;
    public string PasswordText { get; set; } = default!;
}
