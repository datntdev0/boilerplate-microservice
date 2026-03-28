using System.Net.Http.Json;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Admin.Contracts.Tenancy.Dto;

namespace datntdev.Microservice.Tests.Srv.Admin.Tenancy;

[TestClass]
public class TenantsAppServiceTests : MicroserviceSrvAdminBaseTest
{
    private const string BaseUrl = "/api/tenants";

    #region Create Tests

    [TestMethod]
    public async Task CreateAsync_WithValidData_ReturnsCreatedTenant()
    {
        // Arrange
        var createDto = new TenantCreateDto
        {
            Name = $"TestTenant_{Guid.NewGuid():N}",
            Organization = "Test Organization"
        };

        // Act
        using var response = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<TenantDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(createDto.Name, result.Name);
        Assert.AreEqual(createDto.Organization, result.Organization);
        Assert.IsTrue(result.Id != 0);
        Assert.IsNotNull(result.CreatedAt);
    }

    [TestMethod]
    public async Task CreateAsync_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new TenantCreateDto
        {
            Name = string.Empty,
            Organization = "Test Organization"
        };

        // Act
        using var response = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task CreateAsync_WithDuplicateName_ReturnsBadRequest()
    {
        // Arrange
        var uniqueName = $"DuplicateTenant_{Guid.NewGuid():N}";
        var createDto = new TenantCreateDto
        {
            Name = uniqueName,
            Organization = "Test Organization 1"
        };

        // Act - Create first tenant
        using var firstResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        Assert.AreEqual(HttpStatusCode.OK, firstResponse.StatusCode);

        // Act - Try to create duplicate
        var duplicateDto = new TenantCreateDto
        {
            Name = uniqueName,
            Organization = "Test Organization 2"
        };
        using var duplicateResponse = await HttpClient.PostAsJsonAsync(BaseUrl, duplicateDto, CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, duplicateResponse.StatusCode);
    }

    #endregion

    #region Get Tests

    [TestMethod]
    public async Task GetAsync_WithValidId_ReturnsTenant()
    {
        // Arrange - Create a tenant first
        var createDto = new TenantCreateDto
        {
            Name = $"GetTestTenant_{Guid.NewGuid():N}",
            Organization = "Get Test Organization"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdTenant = await createResponse.Content.ReadFromJsonAsync<TenantDto>(CancellationToken);

        // Act
        using var response = await HttpClient.GetAsync($"{BaseUrl}/{createdTenant!.Id}", CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<TenantDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(createdTenant.Id, result.Id);
        Assert.AreEqual(createdTenant.Name, result.Name);
        Assert.AreEqual(createdTenant.Organization, result.Organization);
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
        // Arrange - Create multiple tenants
        for (int i = 0; i < 3; i++)
        {
            var createDto = new TenantCreateDto
            {
                Name = $"PagedTenant_{Guid.NewGuid():N}",
                Organization = $"Organization {i}"
            };
            await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        }

        // Act
        using var response = await HttpClient.GetAsync($"{BaseUrl}?offset=0&limit=10", CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<TenantListDto>>(CancellationToken);

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
        // Arrange - Ensure we have enough tenants
        for (int i = 0; i < 5; i++)
        {
            var createDto = new TenantCreateDto
            {
                Name = $"PaginationTenant_{Guid.NewGuid():N}",
                Organization = $"Organization {i}"
            };
            await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        }

        // Act
        using var response = await HttpClient.GetAsync($"{BaseUrl}?offset=2&limit=3", CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<TenantListDto>>(CancellationToken);

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
    public async Task UpdateAsync_WithValidData_ReturnsUpdatedTenant()
    {
        // Arrange - Create a tenant first
        var createDto = new TenantCreateDto
        {
            Name = $"UpdateTestTenant_{Guid.NewGuid():N}",
            Organization = "Original Organization"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdTenant = await createResponse.Content.ReadFromJsonAsync<TenantDto>(CancellationToken);

        var updateDto = new TenantUpdateDto
        {
            Name = $"UpdatedTenant_{Guid.NewGuid():N}",
            Organization = "Updated Organization"
        };

        // Act
        using var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{createdTenant!.Id}", updateDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<TenantDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.AreEqual(createdTenant.Id, result.Id);
        Assert.AreEqual(updateDto.Name, result.Name);
        Assert.AreEqual(updateDto.Organization, result.Organization);
        Assert.IsNotNull(result.UpdatedAt);
    }

    [TestMethod]
    public async Task UpdateAsync_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange - Create a tenant first
        var createDto = new TenantCreateDto
        {
            Name = $"UpdateValidationTenant_{Guid.NewGuid():N}",
            Organization = "Test Organization"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdTenant = await createResponse.Content.ReadFromJsonAsync<TenantDto>(CancellationToken);

        var updateDto = new TenantUpdateDto
        {
            Name = string.Empty,
            Organization = "Updated Organization"
        };

        // Act
        using var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{createdTenant!.Id}", updateDto, CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task UpdateAsync_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = int.MaxValue;
        var updateDto = new TenantUpdateDto
        {
            Name = "Updated Name",
            Organization = "Updated Organization"
        };

        // Act
        using var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{invalidId}", updateDto, CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task UpdateAsync_WithDuplicateName_ReturnsBadRequest()
    {
        // Arrange - Create two tenants
        var tenant1Name = $"Tenant1_{Guid.NewGuid():N}";
        var tenant2Name = $"Tenant2_{Guid.NewGuid():N}";

        var createDto1 = new TenantCreateDto { Name = tenant1Name, Organization = "Org 1" };
        var createDto2 = new TenantCreateDto { Name = tenant2Name, Organization = "Org 2" };

        using var response1 = await HttpClient.PostAsJsonAsync(BaseUrl, createDto1, CancellationToken);
        var tenant1 = await response1.Content.ReadFromJsonAsync<TenantDto>(CancellationToken);

        using var response2 = await HttpClient.PostAsJsonAsync(BaseUrl, createDto2, CancellationToken);
        var tenant2 = await response2.Content.ReadFromJsonAsync<TenantDto>(CancellationToken);

        // Act - Try to update tenant2 with tenant1's name
        var updateDto = new TenantUpdateDto
        {
            Name = tenant1Name,
            Organization = "Updated Org"
        };
        using var updateResponse = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{tenant2!.Id}", updateDto, CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, updateResponse.StatusCode);
    }

    #endregion

    #region Delete Tests

    [TestMethod]
    public async Task DeleteAsync_WithValidId_DeletesTenant()
    {
        // Arrange - Create a tenant first
        var createDto = new TenantCreateDto
        {
            Name = $"DeleteTestTenant_{Guid.NewGuid():N}",
            Organization = "Delete Test Organization"
        };
        using var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto, CancellationToken);
        var createdTenant = await createResponse.Content.ReadFromJsonAsync<TenantDto>(CancellationToken);

        // Act
        using var deleteResponse = await HttpClient.DeleteAsync($"{BaseUrl}/{createdTenant!.Id}", CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode);

        // Verify tenant is deleted
        using var getResponse = await HttpClient.GetAsync($"{BaseUrl}/{createdTenant.Id}", CancellationToken);
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
