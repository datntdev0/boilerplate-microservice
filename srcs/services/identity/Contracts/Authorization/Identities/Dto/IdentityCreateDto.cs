namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;

public class IdentityCreateDto
{
    public string EmailAddress { get; set; } = default!;
    public string PasswordText { get; set; } = default!;
}
