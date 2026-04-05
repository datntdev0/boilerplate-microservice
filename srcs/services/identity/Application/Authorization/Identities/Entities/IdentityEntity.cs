using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Users.Entities;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Identities.Entities;

public class IdentityEntity : FullAuditEntity<long>
{
    public long UserId { get; set; }
    public string EmailAddress { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string PasswordText { get; set; } = default!;

    public UserEntity User { get; set; } = default!;
}
