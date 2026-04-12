using System.Net.Http.Json;
using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Roles.Dto;

namespace datntdev.Microservice.Tests.Srv.Identity.Authorization.Roles;

[TestClass]
public class RolesAppServiceTests : MicroserviceSrvIdentityBaseTest
{
    private const string BaseUrl = "/api/roles";

    #region Create Tests

    [TestMethod]
    public async Task CreateAsync_WithValidData_ReturnsCreatedRole()
    {
        // Arrange
        var createDto = new RoleCreateDto
        {
            Name = $"TestRole_{Guid.NewGuid():N}",
            Description = "Test Role Description",
            Permissions = [Constants.Permissions.Roles_Read]
        };

        // Act
        using var response = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<RoleDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(createDto.Name, result.Name);
        Assert.AreEqual(createDto.Description, result.Description);
        Assert.IsTrue(result.Id != 0);
        Assert.IsNotNull(result.CreatedAt);
        CollectionAssert.Contains(result.Permissions, Constants.Permissions.Roles_Read);
    }

    [TestMethod]
    public async Task CreateAsync_WithPermissions_ReturnsRoleWithPermissions()
    {
        // Arrange
        var createDto = new RoleCreateDto
        {
            Name = $"PermRole_{Guid.NewGuid():N}",
            Description = "Permissions Test",
            Permissions = [Constants.Permissions.Users_Read, Constants.Permissions.Roles_Read]
        };

        // Act
        using var response = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<RoleDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Permissions.Length);
        CollectionAssert.Contains(result.Permissions, Constants.Permissions.Users_Read);
        CollectionAssert.Contains(result.Permissions, Constants.Permissions.Roles_Read);
    }

    [TestMethod]
    public async Task CreateAsync_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new RoleCreateDto
        {
            Name = string.Empty,
            Description = "Test Description"
        };

        // Act
        using var response = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Get Tests

    [TestMethod]
    public async Task GetAsync_WithValidId_ReturnsRole()
    {
        // Arrange - Create a role first
        var createDto = new RoleCreateDto
        {
            Name = $"GetTestRole_{Guid.NewGuid():N}",
            Description = "Get Test Description"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdRole = await createResponse.Content.ReadFromJsonAsync<RoleDto>(CancellationToken);

        // Act
        using var response = await HttpClient.GetAsync($"{BaseUrl}/{createdRole!.Id}", CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<RoleDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(createdRole.Id, result.Id);
        Assert.AreEqual(createdRole.Name, result.Name);
        Assert.AreEqual(createdRole.Description, result.Description);
    }

    [TestMethod]
    public async Task GetAsync_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = int.MaxValue;

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
        // Arrange - Create multiple roles
        for (int i = 0; i < 3; i++)
        {
            var createDto = new RoleCreateDto
            {
                Name = $"PagedRole_{Guid.NewGuid():N}",
                Description = $"Description {i}"
            };
            await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        }

        // Act
        using var response = await HttpClient.GetAsync($"{BaseUrl}?offset=0&limit=10", CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<RoleListDto>>(CancellationToken);

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
        // Arrange - Ensure we have enough roles
        for (int i = 0; i < 5; i++)
        {
            var createDto = new RoleCreateDto
            {
                Name = $"PaginationRole_{Guid.NewGuid():N}",
                Description = $"Description {i}"
            };
            await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        }

        // Act
        using var response = await HttpClient.GetAsync($"{BaseUrl}?offset=2&limit=3", CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<RoleListDto>>(CancellationToken);

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
    public async Task UpdateAsync_WithValidData_ReturnsUpdatedRole()
    {
        // Arrange - Create a role first
        var createDto = new RoleCreateDto
        {
            Name = $"UpdateTestRole_{Guid.NewGuid():N}",
            Description = "Original Description"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdRole = await createResponse.Content.ReadFromJsonAsync<RoleDto>(CancellationToken);

        var updateDto = new RoleUpdateDto
        {
            Name = $"UpdatedRole_{Guid.NewGuid():N}",
            Description = "Updated Description",
            Permissions = [Constants.Permissions.Roles_Read, Constants.Permissions.Roles_Write]
        };

        // Act
        using var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{createdRole!.Id}", updateDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<RoleDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(createdRole.Id, result.Id);
        Assert.AreEqual(updateDto.Name, result.Name);
        Assert.AreEqual(updateDto.Description, result.Description);
        Assert.IsNotNull(result.UpdatedAt);
        CollectionAssert.Contains(result.Permissions, Constants.Permissions.Roles_Read);
        CollectionAssert.Contains(result.Permissions, Constants.Permissions.Roles_Write);
    }

    [TestMethod]
    public async Task UpdateAsync_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange - Create a role first
        var createDto = new RoleCreateDto
        {
            Name = $"UpdateValidationRole_{Guid.NewGuid():N}",
            Description = "Test Description"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdRole = await createResponse.Content.ReadFromJsonAsync<RoleDto>(CancellationToken);

        var updateDto = new RoleUpdateDto
        {
            Name = string.Empty,
            Description = "Updated Description"
        };

        // Act
        using var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{createdRole!.Id}", updateDto, CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task UpdateAsync_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = int.MaxValue;
        var updateDto = new RoleUpdateDto
        {
            Name = "Updated Name",
            Description = "Updated Description"
        };

        // Act
        using var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{invalidId}", updateDto, CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Delete Tests

    [TestMethod]
    public async Task DeleteAsync_WithValidId_DeletesRole()
    {
        // Arrange - Create a role first
        var createDto = new RoleCreateDto
        {
            Name = $"DeleteTestRole_{Guid.NewGuid():N}",
            Description = "Delete Test Description"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdRole = await createResponse.Content.ReadFromJsonAsync<RoleDto>(CancellationToken);

        // Act
        using var deleteResponse = await HttpClient.DeleteAsync($"{BaseUrl}/{createdRole!.Id}", CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode);

        // Verify role is deleted
        using var getResponse = await HttpClient.GetAsync($"{BaseUrl}/{createdRole.Id}", CancellationToken);
        Assert.AreEqual(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [TestMethod]
    public async Task DeleteAsync_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = int.MaxValue;

        // Act
        using var response = await HttpClient.DeleteAsync($"{BaseUrl}/{invalidId}", CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion
}
