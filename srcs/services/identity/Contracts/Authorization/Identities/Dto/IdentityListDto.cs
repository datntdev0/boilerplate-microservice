using datntdev.Microservice.Shared.Common.Model;

namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;

public class IdentityListDto : BaseDto<long>
{
    public string EmailAddress { get; set; } = default!;
}
