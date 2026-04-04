using datntdev.Microservice.Shared.Application.Validations;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;
using FluentValidation;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Identities;

internal class IdentityCreatingValidator : DbValidator<IdentityCreateDto, MicroserviceSrvIdentityDbContext>
{
    public IdentityCreatingValidator(MicroserviceSrvIdentityDbContext dbContext) : base(dbContext)
    {
        RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.PasswordText).NotEmpty().MinimumLength(6).MaximumLength(100);
    }
}

internal class IdentityUpdatingValidator : DbValidator<IdentityUpdateDto, MicroserviceSrvIdentityDbContext>
{
    public IdentityUpdatingValidator(MicroserviceSrvIdentityDbContext dbContext) : base(dbContext)
    {
        RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.PasswordText).NotEmpty().MinimumLength(6).MaximumLength(100);
    }
}
