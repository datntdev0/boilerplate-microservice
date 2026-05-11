using datntdev.Microservice.Srv.Admin.Application;
using datntdev.Microservice.Srv.Identity.Application;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Identities.Entities;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Permissions;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Roles.Entities;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Users.Entities;
using datntdev.Microservice.Shared.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using datntdev.Microservice.Srv.Admin.Application.Tenancy.Entities;

namespace datntdev.Microservice.Infra.Migrator.Seeders;

internal class MicroserviceSrvIdentitySeeder(IServiceProvider services)
{
    private readonly IConfiguration _configuration = services.GetRequiredService<IConfiguration>();
    private readonly MicroserviceSrvIdentityDbContext _dbContext = services.GetRequiredService<MicroserviceSrvIdentityDbContext>();
    private readonly MicroserviceSrvAdminDbContext _adminDbContext = services.GetRequiredService<MicroserviceSrvAdminDbContext>();
    private readonly PermissionAppProvider _permissionProvider = services.GetRequiredService<PermissionAppProvider>();

    private TenantEntity? _defaultTenantEntity;
    private UserEntity? _defaultAdminUserEntity;

    public async Task SeedAsync()
    {
        await EnsureDefaultHostRolesExistAsync();
        await EnsureDefaultTenantRolesExistAsync();
        await EnsureDefaultAdminIdentityExistsAsync();
        await EnsureTestingIdentityExistsAsync();
        await AssignDefaultAdminUserToTenantsAsync();
        await AssignDefaultAdminUserToRolesAsync();
    }

    private async Task EnsureDefaultHostRolesExistAsync()
    {
        // Host Admin role
        var hostAdminRole = await _dbContext.AppRoles.FirstOrDefaultAsync(r
            => r.Name == Constants.DefaultRoles.HostAdmin && r.TenantId == null);
        if (hostAdminRole != null) _dbContext.AppRoles.Remove(hostAdminRole);

        hostAdminRole = new RoleEntity
        {
            TenantId = null,
            Name = Constants.DefaultRoles.HostAdmin,
            Description = "Host Administrator",
            Permissions = _permissionProvider.GetAllPermissionValues(),
        };
        await _dbContext.AppRoles.AddAsync(hostAdminRole);

        // Host User role
        var hostUserRole = await _dbContext.AppRoles.FirstOrDefaultAsync(r
            => r.Name == Constants.DefaultRoles.HostUser && r.TenantId == null);
        if (hostUserRole != null) _dbContext.AppRoles.Remove(hostUserRole);

        hostUserRole = new RoleEntity
        {
            TenantId = null,
            Name = Constants.DefaultRoles.HostUser,
            Description = "Host User",
        };
        await _dbContext.AppRoles.AddAsync(hostUserRole);

        await _dbContext.SaveChangesAsync();
    }

    private async Task EnsureDefaultTenantRolesExistAsync()
    {
        var defaultTenant = await _adminDbContext.AppTenants.FirstAsync(t => t.Name == Constants.DefaultTenant.Name);
        ArgumentNullException.ThrowIfNull(defaultTenant, nameof(defaultTenant));
        _defaultTenantEntity = defaultTenant;

        // Tenant Admin role
        var tenantAdminRole = await _dbContext.AppRoles
            .Where(x => x.TenantId == defaultTenant.Id)
            .FirstOrDefaultAsync(x => x.Name == Constants.DefaultRoles.TenantAdmin);
        if (tenantAdminRole != null) _dbContext.AppRoles.Remove(tenantAdminRole);

        tenantAdminRole = new RoleEntity
        {
            TenantId = defaultTenant.Id,
            Name = Constants.DefaultRoles.TenantAdmin,
            Description = "Tenant Administrator",
            Permissions = _permissionProvider.GetAllPermissions()
                .Where(p => p.TenancySides == Constants.TenancySides.Tenant)
                .Select(p => p.Permission).ToArray(),
        };
        await _dbContext.AppRoles.AddAsync(tenantAdminRole);

        // Tenant User role
        var tenantUserRole = await _dbContext.AppRoles
            .Where(x => x.TenantId == defaultTenant.Id)
            .FirstOrDefaultAsync(x => x.Name == Constants.DefaultRoles.TenantUser);
        if (tenantUserRole != null) _dbContext.AppRoles.Remove(tenantUserRole);

        tenantUserRole = new RoleEntity
        {
            TenantId = defaultTenant.Id,
            Name = Constants.DefaultRoles.TenantUser,
            Description = "Tenant User",
        };
        await _dbContext.AppRoles.AddAsync(tenantUserRole);

        await _dbContext.SaveChangesAsync();
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

        _defaultAdminUserEntity = new UserEntity
        {
            FirstName = defaultAdminFirstName,
            LastName = defaultAdminLastName,
            Permissions = allPermissions,
        };

        var newIdentity = new IdentityEntity
        {
            EmailAddress = defaultAdminEmail,
            PasswordText = defaultAdminPassword,
            User = _defaultAdminUserEntity
        };

        newIdentity = passwordHasher.SetPassword(newIdentity, defaultAdminPassword);

        await _dbContext.AppIdentities.AddAsync(newIdentity);

        await _dbContext.SaveChangesAsync();
    }

    private async Task EnsureTestingIdentityExistsAsync()
    {
        var testUserEmail = "test@datntdev.com";
        var testUserPassword = "Test@123";
        var testUserFirstName = "Test";
        var testUserLastName = "User";

        var passwordHasher = new Srv.Identity.Application.Authorization.Identities.PasswordHasher();
        var basicPermissions = _permissionProvider.GetAllPermissions()
            .Where(p => p.TenancySides == Constants.TenancySides.Host)
            .Select(p => p.Permission).ToArray();

        // Recreate the test user if it exists
        var existingIdentity = await _dbContext.AppIdentities.FirstOrDefaultAsync(x => x.EmailAddress == testUserEmail);
        if (existingIdentity != null) _dbContext.AppIdentities.Remove(existingIdentity);

        var testUserEntity = new UserEntity
        {
            FirstName = testUserFirstName,
            LastName = testUserLastName,
            Permissions = basicPermissions,
        };

        var testIdentity = new IdentityEntity
        {
            EmailAddress = testUserEmail,
            PasswordText = testUserPassword,
            User = testUserEntity
        };

        testIdentity = passwordHasher.SetPassword(testIdentity, testUserPassword);

        await _dbContext.AppIdentities.AddAsync(testIdentity);
        await _dbContext.SaveChangesAsync();
    }

    private async Task AssignDefaultAdminUserToTenantsAsync()
    {
        // Delete existing user-tenant associations for this user
        var existingUserTenants = await _dbContext.AppUserTenants
            .Where(ut => ut.UserId == _defaultAdminUserEntity!.Id)
            .ToListAsync();
        if (existingUserTenants.Any())
        {
            _dbContext.AppUserTenants.RemoveRange(existingUserTenants);
            await _dbContext.SaveChangesAsync();
        }

        // Create new user-tenant association
        var userTenant = new UserTenantEntity
        {
            UserId = _defaultAdminUserEntity!.Id,
            TenantId = _defaultTenantEntity!.Id,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };
        await _dbContext.AppUserTenants.AddAsync(userTenant);
        await _dbContext.SaveChangesAsync();
    }

    private async Task AssignDefaultAdminUserToRolesAsync()
    {
        // Delete existing user-role associations for this user
        var existingUserRoles = await _dbContext.AppUserRoles
            .Where(ur => ur.UserId == _defaultAdminUserEntity!.Id)
            .ToListAsync();
        if (existingUserRoles.Any())
        {
            _dbContext.AppUserRoles.RemoveRange(existingUserRoles);
            await _dbContext.SaveChangesAsync();
        }

        // Get all Admin roles
        var adminRoles = new string[] { Constants.DefaultRoles.HostAdmin, Constants.DefaultRoles.TenantAdmin };
        var roles = await _dbContext.AppRoles.Where(r => adminRoles.Contains(r.Name)).ToListAsync();

        // Create new user-role associations
        foreach (var role in roles)
        {
            var userRole = new UserRoleEntity
            {
                UserId = _defaultAdminUserEntity!.Id,
                RoleId = role.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };
            await _dbContext.AppUserRoles.AddAsync(userRole);
        }

        await _dbContext.SaveChangesAsync();
    }
}
