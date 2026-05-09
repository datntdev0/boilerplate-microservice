namespace datntdev.Microservice.Shared.Common.Model;

public class SessionModel
{
    public SessionAppModel App { get; set; } = new();
    public SessionUserModel? User { get; set; }
}

public class SessionAppModel
{
    public string Name { get; set; } = Constants.Application.Name;
    public string Version { get; set; } = Constants.Application.Version;
}

public class SessionUserModel
{
    public long Id { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Constants.Permissions[] Permissions { get; set; } = [];
}

public class SessionRoleModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Constants.Permissions[] Permissions { get; set; } = [];
}