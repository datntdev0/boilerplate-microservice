using datntdev.Microservice.Shared.Common.Model;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Users.Entities;

public class UserEntity : FullAuditEntity<long>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
