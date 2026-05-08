using datntdev.Microservice.Shared.Common;

namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;

public class SessionDto
{
    public SessionAppDto App { get; set; } = new();
    public SessionUserDto? User { get; set; }
}

public class SessionAppDto
{
    public string Name { get; set; } = Constants.Application.Name;
    public string Version { get; set; } = Constants.Application.Version;
}

public class SessionUserDto
{
    public long Id { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Constants.Permissions[] Permissions { get; set; } = [];
}

public class SessionRoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Constants.Permissions[] Permissions { get; set; } = [];
}