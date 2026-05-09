using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Shared.Communication.HttpClients;
using Mapster;

namespace datntdev.Microservice.Shared.Application.Authorization;

public class SessionAppProvider(SrvIdentityHttpClient httpClient) : BaseScopedAppProvider
{
    private readonly SrvIdentityHttpClient _httpClient = httpClient;

    public async Task<SessionModel> GetSessionAsync()
    {
        var sessionDto = await _httpClient.Identities_GetSessionAsync();
        return sessionDto.Adapt<SessionModel>();
    }
}