namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Tenants.Dto;

public class TenantUsersInviteResultDto
{
    public string[] RecognizedEmails { get; set; } = [];
    public string[] UnrecognizedEmails { get; set; } = [];
}
