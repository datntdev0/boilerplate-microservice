namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;

public class SigninDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
