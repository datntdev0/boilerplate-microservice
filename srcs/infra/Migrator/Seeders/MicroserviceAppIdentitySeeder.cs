using datntdev.Microservice.App.Identity;
using datntdev.Microservice.App.Identity.Identity;
using datntdev.Microservice.App.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Infra.Migrator.Seeders;

internal class MicroserviceAppIdentitySeeder(IServiceProvider services)
{
    private readonly IConfiguration _configuration = services.GetRequiredService<IConfiguration>();
    private readonly MicroserviceAppIdentityDbContext _dbContext = services.GetRequiredService<MicroserviceAppIdentityDbContext>();

    public async Task SeedAsync()
    {
        await EnsureDefaultAdminIdentityExistsAsync();
    }

    private async Task EnsureDefaultAdminIdentityExistsAsync()
    {
        var defaultAdminEmail = _configuration.GetValue<string>("DefaultAdmin:EmailAddress");
        var defaultAdminPassword = _configuration.GetValue<string>("DefaultAdmin:Password");

        ArgumentNullException.ThrowIfNull(defaultAdminEmail, nameof(defaultAdminEmail));
        ArgumentNullException.ThrowIfNull(defaultAdminPassword, nameof(defaultAdminPassword));

        var passwordHasher = new PasswordHasher();

        // Recreate the default admin user althgough it exists.
        var existingIdentity = await _dbContext.AppIdentities.FirstOrDefaultAsync(x => x.EmailAddress == defaultAdminEmail);
        if (existingIdentity != null) _dbContext.AppIdentities.Remove(existingIdentity);

        var newIdentity = new AppIdentityEntity
        {
            EmailAddress = defaultAdminEmail,
            PasswordText = defaultAdminPassword,
        };

        newIdentity = passwordHasher.SetPassword(newIdentity, defaultAdminPassword);

        await _dbContext.AppIdentities.AddAsync(newIdentity);
        await _dbContext.SaveChangesAsync();
    }
}
