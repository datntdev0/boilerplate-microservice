namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;

public class SignupDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
