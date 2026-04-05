using System.Net.Http.Json;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;

namespace datntdev.Microservice.Tests.Srv.Identity.Authorization.Identities;

[TestClass]
public class IdentitiesAppServiceTests : MicroserviceSrvIdentityBaseTest
{
    private const string BaseUrl = "/api/identities";

    #region Create Tests

    [TestMethod]
    public async Task CreateAsync_WithValidData_ReturnsCreatedIdentity()
    {
        // Arrange
        var createDto = new IdentityCreateDto
        {
            EmailAddress = $"test_{Guid.NewGuid():N}@example.com",
            PasswordText = "SecurePassword123!"
        };

        // Act
        using var response = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<IdentityDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(createDto.EmailAddress, result.EmailAddress);
        Assert.IsTrue(result.Id != 0);
        Assert.IsNotNull(result.PasswordHash);
        Assert.AreNotEqual(createDto.PasswordText, result.PasswordHash);
        Assert.IsNotNull(result.CreatedAt);
    }

    [TestMethod]
    public async Task CreateAsync_WithEmptyEmail_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new IdentityCreateDto
        {
            EmailAddress = string.Empty,
            PasswordText = "Password123!"
        };

        // Act
        using var response = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Get Tests

    [TestMethod]
    public async Task GetAsync_WithValidId_ReturnsIdentity()
    {
        // Arrange - Create an identity first
        var createDto = new IdentityCreateDto
        {
            EmailAddress = $"get_test_{Guid.NewGuid():N}@example.com",
            PasswordText = "TestPassword123!"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdIdentity = await createResponse.Content.ReadFromJsonAsync<IdentityDto>(CancellationToken);

        // Act
        using var response = await HttpClient.GetAsync($"{BaseUrl}/{createdIdentity!.Id}", CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<IdentityDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(createdIdentity.Id, result.Id);
        Assert.AreEqual(createdIdentity.EmailAddress, result.EmailAddress);
        Assert.AreEqual(createdIdentity.PasswordHash, result.PasswordHash);
    }

    [TestMethod]
    public async Task GetAsync_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = long.MaxValue;

        // Act
        using var response = await HttpClient.GetAsync($"{BaseUrl}/{invalidId}", CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region GetAll Tests

    [TestMethod]
    public async Task GetAllAsync_WithDefaultPagination_ReturnsPagedResult()
    {
        // Arrange - Create multiple identities
        for (int i = 0; i < 3; i++)
        {
            var createDto = new IdentityCreateDto
            {
                EmailAddress = $"paged_test_{i}_{Guid.NewGuid():N}@example.com",
                PasswordText = $"Password{i}123!"
            };
            await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        }

        // Act
        using var response = await HttpClient.GetAsync($"{BaseUrl}?offset=0&limit=10", CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<IdentityListDto>>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Total >= 3);
        Assert.IsNotNull(result.Items);
        Assert.IsTrue(result.Items.Count() >= 3);
        Assert.AreEqual(10, result.Limit);
        Assert.AreEqual(0, result.Offset);
    }

    [TestMethod]
    public async Task GetAllAsync_WithPagination_ReturnsCorrectPage()
    {
        // Arrange - Ensure we have enough identities
        for (int i = 0; i < 5; i++)
        {
            var createDto = new IdentityCreateDto
            {
                EmailAddress = $"pagination_test_{i}_{Guid.NewGuid():N}@example.com",
                PasswordText = $"Password{i}123!"
            };
            await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        }

        // Act
        using var response = await HttpClient.GetAsync($"{BaseUrl}?offset=2&limit=3", CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<IdentityListDto>>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Limit);
        Assert.AreEqual(2, result.Offset);
        Assert.IsTrue(result.Items.Count() <= 3);
    }

    #endregion

    #region Update Tests

    [TestMethod]
    public async Task UpdateAsync_WithValidData_ReturnsUpdatedIdentity()
    {
        // Arrange - Create an identity first
        var createDto = new IdentityCreateDto
        {
            EmailAddress = $"original_{Guid.NewGuid():N}@example.com",
            PasswordText = "OriginalPassword123!"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdIdentity = await createResponse.Content.ReadFromJsonAsync<IdentityDto>(CancellationToken);

        var updateDto = new IdentityUpdateDto
        {
            EmailAddress = $"updated_{Guid.NewGuid():N}@example.com",
            PasswordText = "NewPassword123!"
        };

        // Act
        using var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{createdIdentity!.Id}", updateDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<IdentityDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(createdIdentity.Id, result.Id);
        Assert.AreEqual(updateDto.EmailAddress, result.EmailAddress);
        Assert.IsNotNull(result.UpdatedAt);
    }

    [TestMethod]
    public async Task UpdateAsync_WithEmptyEmail_ReturnsBadRequest()
    {
        // Arrange - Create an identity first
        var createDto = new IdentityCreateDto
        {
            EmailAddress = $"validation_test_{Guid.NewGuid():N}@example.com",
            PasswordText = "Password123!"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdIdentity = await createResponse.Content.ReadFromJsonAsync<IdentityDto>(CancellationToken);

        var updateDto = new IdentityUpdateDto
        {
            EmailAddress = string.Empty,
            PasswordText = "Password123!"
        };

        // Act
        using var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{createdIdentity!.Id}", updateDto, CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task UpdateAsync_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = long.MaxValue;
        var updateDto = new IdentityUpdateDto
        {
            EmailAddress = $"test_{Guid.NewGuid():N}@example.com",
            PasswordText = "Password123!"
        };

        // Act
        using var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{invalidId}", updateDto, CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Delete Tests

    [TestMethod]
    public async Task DeleteAsync_WithValidId_DeletesIdentity()
    {
        // Arrange - Create an identity first
        var createDto = new IdentityCreateDto
        {
            EmailAddress = $"delete_test_{Guid.NewGuid():N}@example.com",
            PasswordText = "Password123!"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdIdentity = await createResponse.Content.ReadFromJsonAsync<IdentityDto>(CancellationToken);

        // Act
        using var deleteResponse = await HttpClient.DeleteAsync($"{BaseUrl}/{createdIdentity!.Id}", CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode);

        // Verify identity is deleted
        using var getResponse = await HttpClient.GetAsync($"{BaseUrl}/{createdIdentity.Id}", CancellationToken);
        Assert.AreEqual(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [TestMethod]
    public async Task DeleteAsync_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = long.MaxValue;

        // Act
        using var response = await HttpClient.DeleteAsync($"{BaseUrl}/{invalidId}", CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion
}
