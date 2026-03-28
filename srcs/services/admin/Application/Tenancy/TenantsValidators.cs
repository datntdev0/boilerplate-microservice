using datntdev.Microservice.Shared.Application.Validations;
using datntdev.Microservice.Srv.Admin.Contracts.Tenancy.Dto;
using FluentValidation;

namespace datntdev.Microservice.Srv.Admin.Application.Tenancy;

internal class TenantCreatingValidator : DbValidator<TenantCreateDto, MicroserviceSrvAdminDbContext>
{
    public TenantCreatingValidator(MicroserviceSrvAdminDbContext dbContext) : base(dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .Must(name => !CheckNameExistence(name))
            .WithMessage(name => $"Tenant name '{name}' already exists.");
    }

    private bool CheckNameExistence(string name)
    {
        return _dbContext.AppTenants.Any(t => t.Name == name);
    }
}

internal class TenantUpdatingValidator : DbValidator<TenantUpdateDto, MicroserviceSrvAdminDbContext>
{
    public TenantUpdatingValidator(MicroserviceSrvAdminDbContext dbContext) : base(dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .Must((dto, name) => !CheckNameExistence(dto.Id, name))
            .WithMessage((dto, name) => $"Tenant name '{name}' already exists.");
    }
    private bool CheckNameExistence(int id, string name)
    {
        return _dbContext.AppTenants.Any(t => t.Name == name && t.Id != id);
    }
}
