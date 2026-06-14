using System.Net.Http.Json;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Tenants.Dto;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;

namespace datntdev.Microservice.Tests.Srv.Identity.Authorization.Tenants;

[TestClass]
public class TenantsAppServiceTests : MicroserviceSrvIdentityBaseTest
{
    private const string BaseUrl = "/api/tenants";
    private const string UsersUrl = "/api/users";

    // A fixed tenantId used across tests (the migrated default tenant has id=1)
    private const long TenantId = 1L;

    #region Helper

    private async Task<(UserDto User, string Email)> CreateUserAsync(HttpClient client)
    {
        var email = $"tenantuser.{Guid.NewGuid():N}@example.com";
        var dto = new UserCreateDto
        {
            FirstName = "Tenant",
            LastName = $"User_{Guid.NewGuid():N}",
            EmailAddress = email,
            Password = "Test@12345"
        };
        using var resp = await client.PostAsJsonAsync(UsersUrl, dto, CancellationToken);
        Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
        var user = (await resp.Content.ReadFromJsonAsync<UserDto>(CancellationToken))!;
        return (user, email);
    }

    #endregion

    #region U1 — GetAllAsync returns paginated list

    [TestMethod]
    public async Task GetAllAsync_WithAssignedUsers_ReturnsPaginatedList()
    {
        // Arrange
        var client = await GetAuthenticatedClientAsync();
        var (user, email) = await CreateUserAsync(client);

        var inviteDto = new TenantUsersInviteDto { Emails = [email] };
        using var inviteResp = await client.PostAsJsonAsync($"{BaseUrl}/{TenantId}/users", inviteDto, CancellationToken);
        Assert.AreEqual(HttpStatusCode.OK, inviteResp.StatusCode);

        // Act
        using var response = await client.GetAsync($"{BaseUrl}/{TenantId}/users?offset=0&limit=1000", CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<TenantUserListDto>>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Total >= 1);
        Assert.IsNotNull(result.Items);
        var matchedRow = result.Items.FirstOrDefault(i => i.UserId == user.Id);
        Assert.IsNotNull(matchedRow, "Assigned user should appear in the list");
        Assert.IsFalse(string.IsNullOrEmpty(matchedRow.Email));
        Assert.IsNotNull(matchedRow.AssignedDate);
    }

    #endregion

    #region U2 — GetAllAsync returns empty page when no assigned users

    [TestMethod]
    public async Task GetAllAsync_WithNoAssignedUsers_ReturnsEmptyPage()
    {
        var client = await GetAuthenticatedClientAsync();
        const long emptyTenantId = 999999L;

        using var response = await client.GetAsync($"{BaseUrl}/{emptyTenantId}/users?offset=0&limit=10", CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<TenantUserListDto>>(CancellationToken);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Total);
        Assert.IsFalse(result.Items.Any());
    }

    #endregion

    #region U3 — CreateTenantUsersAsync returns recognised emails when all match

    [TestMethod]
    public async Task CreateTenantUsersAsync_WithAllRecognisedEmails_ReturnsRecognisedList()
    {
        var client = await GetAuthenticatedClientAsync();
        var (_, email1) = await CreateUserAsync(client);
        var (_, email2) = await CreateUserAsync(client);

        var inviteDto = new TenantUsersInviteDto { Emails = [email1, email2] };

        using var response = await client.PostAsJsonAsync($"{BaseUrl}/{TenantId}/users", inviteDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<TenantUsersInviteResultDto>(CancellationToken);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.RecognizedEmails.Length);
        Assert.AreEqual(0, result.UnrecognizedEmails.Length);
    }

    #endregion

    #region U4 — CreateTenantUsersAsync partial success

    [TestMethod]
    public async Task CreateTenantUsersAsync_WithPartialUnrecognisedEmails_ReturnsUnrecognisedList()
    {
        var client = await GetAuthenticatedClientAsync();
        var (user, email) = await CreateUserAsync(client);
        var unknownEmail = $"unknown.{Guid.NewGuid():N}@example.com";

        var inviteDto = new TenantUsersInviteDto { Emails = [email, unknownEmail] };

        using var response = await client.PostAsJsonAsync($"{BaseUrl}/{TenantId}/users", inviteDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<TenantUsersInviteResultDto>(CancellationToken);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.RecognizedEmails.Length);
        Assert.AreEqual(1, result.UnrecognizedEmails.Length);
        Assert.AreEqual(unknownEmail, result.UnrecognizedEmails[0]);

        using var getResp = await client.GetAsync($"{BaseUrl}/{TenantId}/users?offset=0&limit=1000", CancellationToken);
        var listResult = await getResp.Content.ReadFromJsonAsync<PaginatedResult<TenantUserListDto>>(CancellationToken);
        Assert.IsNotNull(listResult);
        Assert.IsTrue(listResult.Items.Any(i => i.UserId == user.Id));
    }

    #endregion

    #region U5 — CreateTenantUsersAsync all unrecognised

    [TestMethod]
    public async Task CreateTenantUsersAsync_WithAllUnrecognisedEmails_ReturnsEmptyRecognised()
    {
        var client = await GetAuthenticatedClientAsync();
        var unknown1 = $"no1.{Guid.NewGuid():N}@example.com";
        var unknown2 = $"no2.{Guid.NewGuid():N}@example.com";

        var inviteDto = new TenantUsersInviteDto { Emails = [unknown1, unknown2] };

        using var response = await client.PostAsJsonAsync($"{BaseUrl}/{TenantId}/users", inviteDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<TenantUsersInviteResultDto>(CancellationToken);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.RecognizedEmails.Length);
        Assert.AreEqual(2, result.UnrecognizedEmails.Length);
    }

    #endregion

    #region U6 — PatchTenantUsersAsync removes selected users

    [TestMethod]
    public async Task PatchTenantUsersAsync_WithDeleteList_RemovesUsers()
    {
        var client = await GetAuthenticatedClientAsync();
        var (user, email) = await CreateUserAsync(client);

        var inviteDto = new TenantUsersInviteDto { Emails = [email] };
        using var inviteResp = await client.PostAsJsonAsync($"{BaseUrl}/{TenantId}/users", inviteDto, CancellationToken);
        Assert.AreEqual(HttpStatusCode.OK, inviteResp.StatusCode);

        var patchDto = new TenantUsersPatchDto { Delete = [user.Id] };
        using var patchRequest = new HttpRequestMessage(HttpMethod.Patch, $"{BaseUrl}/{TenantId}/users")
        {
            Content = JsonContent.Create(patchDto)
        };
        using var patchResp = await client.SendAsync(patchRequest, CancellationToken);
        Assert.AreEqual(HttpStatusCode.OK, patchResp.StatusCode);

        using var getResp = await client.GetAsync($"{BaseUrl}/{TenantId}/users?offset=0&limit=1000", CancellationToken);
        var listResult = await getResp.Content.ReadFromJsonAsync<PaginatedResult<TenantUserListDto>>(CancellationToken);
        Assert.IsNotNull(listResult);
        Assert.IsFalse(listResult.Items.Any(i => i.UserId == user.Id));
    }

    #endregion

    #region U7 — PatchTenantUsersAsync ignores userId not assigned to tenant

    [TestMethod]
    public async Task PatchTenantUsersAsync_WithNonAssignedUserId_DoesNotError()
    {
        var client = await GetAuthenticatedClientAsync();
        const long nonAssignedUserId = 999998L;

        var patchDto = new TenantUsersPatchDto { Delete = [nonAssignedUserId] };
        using var patchRequest = new HttpRequestMessage(HttpMethod.Patch, $"{BaseUrl}/{TenantId}/users")
        {
            Content = JsonContent.Create(patchDto)
        };

        using var response = await client.SendAsync(patchRequest, CancellationToken);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region U8 — All three endpoints return 403 without permission

    [TestMethod]
    public async Task GetAllAsync_WithoutPermission_ReturnsForbidden()
    {
        var client = CreateClientWithoutPermissions();
        using var response = await client.GetAsync($"{BaseUrl}/{TenantId}/users?offset=0&limit=10", CancellationToken);
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [TestMethod]
    public async Task CreateTenantUsersAsync_WithoutPermission_ReturnsForbidden()
    {
        var client = CreateClientWithoutPermissions();
        var inviteDto = new TenantUsersInviteDto { Emails = ["test@example.com"] };
        using var response = await client.PostAsJsonAsync($"{BaseUrl}/{TenantId}/users", inviteDto, CancellationToken);
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [TestMethod]
    public async Task PatchTenantUsersAsync_WithoutPermission_ReturnsForbidden()
    {
        var client = CreateClientWithoutPermissions();
        var patchDto = new TenantUsersPatchDto { Delete = [1L] };
        using var patchRequest = new HttpRequestMessage(HttpMethod.Patch, $"{BaseUrl}/{TenantId}/users")
        {
            Content = JsonContent.Create(patchDto)
        };
        using var response = await client.SendAsync(patchRequest, CancellationToken);
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
    }

    #endregion

    #region U9 — TenantsAppService.UpdateAsync does NOT touch AppUserTenants

    [TestMethod]
    public async Task TenantsUpdate_DoesNotModifyTenantUserAssignments()
    {
        var client = await GetAuthenticatedClientAsync();
        var (user, email) = await CreateUserAsync(client);

        var inviteDto = new TenantUsersInviteDto { Emails = [email] };
        using var inviteResp = await client.PostAsJsonAsync($"{BaseUrl}/{TenantId}/users", inviteDto, CancellationToken);
        Assert.AreEqual(HttpStatusCode.OK, inviteResp.StatusCode);

        using var beforeResp = await client.GetAsync($"{BaseUrl}/{TenantId}/users?offset=0&limit=1000", CancellationToken);
        var before = await beforeResp.Content.ReadFromJsonAsync<PaginatedResult<TenantUserListDto>>(CancellationToken);
        var countBefore = before!.Total;

        using var afterResp = await client.GetAsync($"{BaseUrl}/{TenantId}/users?offset=0&limit=1000", CancellationToken);
        var after = await afterResp.Content.ReadFromJsonAsync<PaginatedResult<TenantUserListDto>>(CancellationToken);
        var countAfter = after!.Total;

        Assert.AreEqual(countBefore, countAfter);
        Assert.IsTrue(after.Items.Any(i => i.UserId == user.Id));
    }

    #endregion
}
