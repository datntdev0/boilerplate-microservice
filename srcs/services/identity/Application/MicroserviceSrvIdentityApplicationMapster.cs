using datntdev.Microservice.Srv.Identity.Application.Authorization.Users.Entities;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Tenants.Dto;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;
using Mapster;

namespace datntdev.Microservice.Srv.Identity.Application;

public class MicroserviceSrvIdentityApplicationMapster : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserEntity, SessionUserDto>()
            .Map(dest => dest.EmailAddress, src => src.Identities.First().EmailAddress);

        config.NewConfig<UserEntity, UserListDto>()
            .Map(dest => dest.EmailAddress, src => src.Identities.First().EmailAddress, src => src.Identities.Any());

        config.NewConfig<UserTenantEntity, TenantUserListDto>()
            .Map(dest => dest.Email, src => src.User != null && src.User.Identities.Any()
                ? src.User.Identities.First().EmailAddress
                : string.Empty)
            .Map(dest => dest.FullName, src => src.User != null
                ? $"{src.User.FirstName} {src.User.LastName}".Trim()
                : string.Empty)
            .Map(dest => dest.AssignedDate, src => src.CreatedAt);
    }
}