using datntdev.Microservice.Srv.Identity.Application.Authorization.Identities.Entities;
using Microsoft.AspNetCore.Identity;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Identities;

public class PasswordHasher : PasswordHasher<IdentityEntity>
{
    public IdentityEntity SetPassword(IdentityEntity identityEntity, string password)
    {
        if (!string.IsNullOrEmpty(password))
        {
            identityEntity.PasswordHash = base.HashPassword(identityEntity, password);
        }
        return identityEntity;
    }
}
