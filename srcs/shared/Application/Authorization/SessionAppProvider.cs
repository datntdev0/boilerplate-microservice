using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Shared.Communication.HttpClients;
using Mapster;

namespace datntdev.Microservice.Shared.Application.Authorization;

public class SessionAppProvider(SrvIdentityHttpClient httpClient) : BaseScopedAppProvider
{
    private readonly SrvIdentityHttpClient _httpClient = httpClient;

    private SessionModel? _currentSession;

    public async Task<SessionModel> GetSessionAsync()
    {
        if (_currentSession != null) return _currentSession;
        var sessionDto = await _httpClient.Identities_GetSessionAsync();
        _currentSession = sessionDto.Adapt<SessionModel>();
        return _currentSession;
    }
}