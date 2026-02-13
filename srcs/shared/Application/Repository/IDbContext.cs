namespace datntdev.Microservice.Shared.Application.Repository;

public interface IDbContext
{
}

public interface IRelationalDbContext : IDbContext
{
}

public interface IDocumentDbContext : IDbContext
{
}
