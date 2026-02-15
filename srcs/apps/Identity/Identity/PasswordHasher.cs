using datntdev.Microservice.App.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace datntdev.Microservice.App.Identity.Identity;

public class PasswordHasher : PasswordHasher<AppIdentityEntity>
{
    public AppIdentityEntity SetPassword(AppIdentityEntity identityEntity, string password)
    {
        if (!string.IsNullOrEmpty(password))
        {
            identityEntity.PasswordHash = base.HashPassword(identityEntity, password);
        }
        return identityEntity;
    }
}
