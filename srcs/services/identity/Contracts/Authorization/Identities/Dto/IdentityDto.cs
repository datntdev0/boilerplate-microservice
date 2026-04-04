using datntdev.Microservice.Shared.Common.Model;

namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;

public class IdentityDto : BaseAuditDto<long>
{
    public string EmailAddress { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
}
