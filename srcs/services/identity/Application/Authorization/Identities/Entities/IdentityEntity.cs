using datntdev.Microservice.Shared.Common.Model;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Identities.Entities;

public class IdentityEntity : FullAuditEntity<long>
{
    public string EmailAddress { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string PasswordText { get; set; } = default!;
}
