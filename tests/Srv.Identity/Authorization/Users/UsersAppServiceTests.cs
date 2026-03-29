using System.Net.Http.Json;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;

namespace datntdev.Microservice.Tests.Srv.Identity.Authorization.Users;

[TestClass]
public class UsersAppServiceTests : MicroserviceSrvIdentityBaseTest
{
    private const string BaseUrl = "/api/users";

    #region Create Tests

    [TestMethod]
    public async Task CreateAsync_WithValidData_ReturnsCreatedUser()
    {
        // Arrange
        var createDto = new UserCreateDto
        {
            FirstName = "John",
            LastName = $"Doe_{Guid.NewGuid():N}"
        };

        // Act
        using var response = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<UserDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(createDto.FirstName, result.FirstName);
        Assert.AreEqual(createDto.LastName, result.LastName);
        Assert.IsTrue(result.Id != 0);
        Assert.IsNotNull(result.CreatedAt);
    }

    [TestMethod]
    public async Task CreateAsync_WithEmptyFirstName_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new UserCreateDto
        {
            FirstName = string.Empty,
            LastName = "Doe"
        };

        // Act
        using var response = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Get Tests

    [TestMethod]
    public async Task GetAsync_WithValidId_ReturnsUser()
    {
        // Arrange - Create a user first
        var createDto = new UserCreateDto
        {
            FirstName = "Get",
            LastName = $"Test_{Guid.NewGuid():N}"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>(CancellationToken);

        // Act
        using var response = await HttpClient.GetAsync($"{BaseUrl}/{createdUser!.Id}", CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<UserDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(createdUser.Id, result.Id);
        Assert.AreEqual(createdUser.FirstName, result.FirstName);
        Assert.AreEqual(createdUser.LastName, result.LastName);
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
        // Arrange - Create multiple users
        for (int i = 0; i < 3; i++)
        {
            var createDto = new UserCreateDto
            {
                FirstName = "Paged",
                LastName = $"User{i}_{Guid.NewGuid():N}"
            };
            await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        }

        // Act
        using var response = await HttpClient.GetAsync($"{BaseUrl}?offset=0&limit=10", CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<UserListDto>>(CancellationToken);

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
        // Arrange - Ensure we have enough users
        for (int i = 0; i < 5; i++)
        {
            var createDto = new UserCreateDto
            {
                FirstName = "Pagination",
                LastName = $"User{i}_{Guid.NewGuid():N}"
            };
            await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        }

        // Act
        using var response = await HttpClient.GetAsync($"{BaseUrl}?offset=2&limit=3", CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<UserListDto>>(CancellationToken);

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
    public async Task UpdateAsync_WithValidData_ReturnsUpdatedUser()
    {
        // Arrange - Create a user first
        var createDto = new UserCreateDto
        {
            FirstName = "Original",
            LastName = $"Name_{Guid.NewGuid():N}"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>(CancellationToken);

        var updateDto = new UserUpdateDto
        {
            FirstName = "Updated",
            LastName = $"Name_{Guid.NewGuid():N}"
        };

        // Act
        using var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{createdUser!.Id}", updateDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<UserDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(createdUser.Id, result.Id);
        Assert.AreEqual(updateDto.FirstName, result.FirstName);
        Assert.AreEqual(updateDto.LastName, result.LastName);
        Assert.IsNotNull(result.UpdatedAt);
    }

    [TestMethod]
    public async Task UpdateAsync_WithEmptyFirstName_ReturnsBadRequest()
    {
        // Arrange - Create a user first
        var createDto = new UserCreateDto
        {
            FirstName = "Validation",
            LastName = $"Test_{Guid.NewGuid():N}"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>(CancellationToken);

        var updateDto = new UserUpdateDto
        {
            FirstName = string.Empty,
            LastName = "Test"
        };

        // Act
        using var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{createdUser!.Id}", updateDto, CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task UpdateAsync_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = long.MaxValue;
        var updateDto = new UserUpdateDto
        {
            FirstName = "Updated",
            LastName = "Name"
        };

        // Act
        using var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{invalidId}", updateDto, CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Delete Tests

    [TestMethod]
    public async Task DeleteAsync_WithValidId_DeletesUser()
    {
        // Arrange - Create a user first
        var createDto = new UserCreateDto
        {
            FirstName = "Delete",
            LastName = $"Test_{Guid.NewGuid():N}"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>(CancellationToken);

        // Act
        using var deleteResponse = await HttpClient.DeleteAsync($"{BaseUrl}/{createdUser!.Id}", CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode);

        // Verify user is deleted
        using var getResponse = await HttpClient.GetAsync($"{BaseUrl}/{createdUser.Id}", CancellationToken);
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
