using datntdev.Microservice.Shared.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using IdentityResult = datntdev.Microservice.App.Identity.Models.IdentityResult;

namespace datntdev.Microservice.App.Identity.Identity;

public class IdentityManager(IServiceProvider services) 
    : MicroserviceAppIdentityBaseManager(services)
{
    private readonly IHttpContextAccessor _contextAccessor = services.GetRequiredService<IHttpContextAccessor>();
    private readonly PasswordHasher _passwordHasher = services.GetRequiredService<PasswordHasher>();

    public async Task<IdentityResult> SignInWithPassword(string email, string password)
    {
        var identityEntity = await _dbContext.AppIdentities
            .FirstOrDefaultAsync(x => x.EmailAddress == email);
        if (identityEntity == null) return IdentityResult.Failure;

        var passwordVerification = _passwordHasher.VerifyHashedPassword(
            identityEntity, identityEntity.PasswordHash, password);

        if (passwordVerification == PasswordVerificationResult.Success)
        {
            var claims = new Claim[]
            {
                new(ClaimTypes.Name, identityEntity.EmailAddress),
                new(ClaimTypes.NameIdentifier, identityEntity.Id.ToString()),
                new(ClaimTypes.Email, identityEntity.EmailAddress ?? string.Empty),
            };

            var claimsIdentity = new ClaimsIdentity(claims, Constants.Application.AuthenticationScheme);
            await _contextAccessor.HttpContext!.SignInAsync(new ClaimsPrincipal(claimsIdentity));
        }

        return passwordVerification switch
        {
            PasswordVerificationResult.Success => IdentityResult.Success,
            PasswordVerificationResult.Failed => IdentityResult.Failure,
            _ => throw new NotImplementedException(),
        };
    }

    public async Task<IdentityResult> SignUpWithPassword(string email, string password)
    {
        var identityEntity = await _dbContext.AppIdentities
            .FirstOrDefaultAsync(x => x.EmailAddress == email);
        if (identityEntity != null) return IdentityResult.Duplicated;

        identityEntity = new Models.AppIdentityEntity()
        {
            EmailAddress = email,
            PasswordText = password,
        };
        identityEntity = _passwordHasher.SetPassword(identityEntity, password);

        _dbContext.AppIdentities.Add(identityEntity);
        await _dbContext.SaveChangesAsync();
        return IdentityResult.Success;
    }
}
