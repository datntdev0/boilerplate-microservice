using datntdev.Microservice.Shared.Application.Validations;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Roles.Dto;
using FluentValidation;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Roles;

internal class RoleCreatingValidator : DbValidator<RoleCreateDto, MicroserviceSrvIdentityDbContext>
{
    public RoleCreatingValidator(MicroserviceSrvIdentityDbContext dbContext) : base(dbContext)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    }
}

internal class RoleUpdatingValidator : DbValidator<RoleUpdateDto, MicroserviceSrvIdentityDbContext>
{
    public RoleUpdatingValidator(MicroserviceSrvIdentityDbContext dbContext) : base(dbContext)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    }
}
