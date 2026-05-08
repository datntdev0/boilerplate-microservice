using datntdev.Microservice.Srv.Identity.Application.Authorization.Users.Entities;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;
using Mapster;

namespace datntdev.Microservice.Srv.Identity.Application;

public class MicroserviceSrvIdentityApplicationMapster : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserEntity, SessionUserDto>()
            .Map(dest => dest.EmailAddress, src => src.Identities.First().EmailAddress);
    }
}