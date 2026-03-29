using datntdev.Microservice.App.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace datntdev.Microservice.App.Identity.Identity;

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
