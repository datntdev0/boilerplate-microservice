using datntdev.Microservice.Shared.Common.Model;

namespace datntdev.Microservice.App.Identity.Models;

public class AppIdentityEntity : FullAuditEntity<long>
{
    public long UserId { get; set; }
    public string EmailAddress { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string PasswordText { get; set; } = default!;
}
