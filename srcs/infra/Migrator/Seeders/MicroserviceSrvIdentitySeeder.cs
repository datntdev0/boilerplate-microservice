using datntdev.Microservice.Srv.Identity.Application;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Identities.Entities;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Permissions;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Infra.Migrator.Seeders;

internal class MicroserviceSrvIdentitySeeder(IServiceProvider services)
{
    private readonly IConfiguration _configuration = services.GetRequiredService<IConfiguration>();
    private readonly MicroserviceSrvIdentityDbContext _dbContext = services.GetRequiredService<MicroserviceSrvIdentityDbContext>();

    private readonly PermissionAppProvider _permissionProvider = services.GetRequiredService<PermissionAppProvider>();

    public async Task SeedAsync()
    {
        await EnsureDefaultAdminIdentityExistsAsync();
    }

    private async Task EnsureDefaultAdminIdentityExistsAsync()
    {
        var defaultAdminEmail = _configuration.GetValue<string>("DefaultAdmin:EmailAddress");
        var defaultAdminPassword = _configuration.GetValue<string>("DefaultAdmin:Password");
        var defaultAdminFirstName = _configuration.GetValue<string>("DefaultAdmin:FirstName") ?? string.Empty;
        var defaultAdminLastName = _configuration.GetValue<string>("DefaultAdmin:LastName") ?? string.Empty;

        ArgumentNullException.ThrowIfNull(defaultAdminEmail, nameof(defaultAdminEmail));
        ArgumentNullException.ThrowIfNull(defaultAdminPassword, nameof(defaultAdminPassword));

        var passwordHasher = new Srv.Identity.Application.Authorization.Identities.PasswordHasher();
        var allPermissions = _permissionProvider.GetAllPermissions().Select(x => x.Permission).ToArray();
        // Recreate the default admin user althgough it exists.
        var existingIdentity = await _dbContext.AppIdentities.FirstOrDefaultAsync(x => x.EmailAddress == defaultAdminEmail);
        if (existingIdentity != null) _dbContext.AppIdentities.Remove(existingIdentity);

        var newIdentity = new IdentityEntity
        {
            EmailAddress = defaultAdminEmail,
            PasswordText = defaultAdminPassword,
            User = new UserEntity
            {
                FirstName = defaultAdminFirstName,
                LastName = defaultAdminLastName,
                Permissions = allPermissions,
            }
        };

        newIdentity = passwordHasher.SetPassword(newIdentity, defaultAdminPassword);

        await _dbContext.AppIdentities.AddAsync(newIdentity);

        await _dbContext.SaveChangesAsync();
    }
}
