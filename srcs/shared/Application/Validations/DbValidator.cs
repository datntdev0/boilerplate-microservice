using datntdev.Microservice.Shared.Application.Repository;
using FluentValidation;

namespace datntdev.Microservice.Shared.Application.Validations;
public abstract class DbValidator<TContract, TDbContext>(TDbContext dbContext) : AbstractValidator<TContract>
     where TDbContext : BaseDbContext
{
    protected readonly TDbContext _dbContext = dbContext;
}
