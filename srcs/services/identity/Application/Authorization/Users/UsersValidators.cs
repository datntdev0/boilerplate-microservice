using datntdev.Microservice.Shared.Application.Validations;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;
using FluentValidation;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Users;

internal class UserCreatingValidator : DbValidator<UserCreateDto, MicroserviceSrvIdentityDbContext>
{
    public UserCreatingValidator(MicroserviceSrvIdentityDbContext dbContext) : base(dbContext)
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
    }
}

internal class UserUpdatingValidator : DbValidator<UserUpdateDto, MicroserviceSrvIdentityDbContext>
{
    public UserUpdatingValidator(MicroserviceSrvIdentityDbContext dbContext) : base(dbContext)
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
    }
}
