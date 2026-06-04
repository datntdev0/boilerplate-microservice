using System.Net.Http.Json;
using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;

namespace datntdev.Microservice.Tests.Srv.Identity.Authorization.Identities;

[TestClass]
public class IdentitiesAppServiceTests : MicroserviceSrvIdentityBaseTest
{
    private const string SessionUrl = "/api/identities/session";
    private const string SigninUrl = "/api/identities/signin";
    private const string SignupUrl = "/api/identities/signup";

    #region Signin Tests

    [TestMethod]
    public async Task CreateSigninAsync_WithValidCredentials_ReturnsUserDto()
    {
        // Arrange - First create a user via signup
        var signupDto = new SignupDto
        {
            Email = $"signin.valid.{Guid.NewGuid():N}@test.com",
            Password = "Test@123456",
            FirstName = "Signin",
            LastName = "Valid"
        };
        await HttpClient.PostAsJsonAsync(SignupUrl, signupDto, CancellationToken);

        var signinDto = new SigninDto
        {
            Email = signupDto.Email,
            Password = signupDto.Password
        };

        // Act
        using var response = await HttpClient.PostAsJsonAsync(SigninUrl, signinDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<UserDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Id > 0);
        Assert.AreEqual(signupDto.FirstName, result.FirstName);
        Assert.AreEqual(signupDto.LastName, result.LastName);
    }

    [TestMethod]
    public async Task CreateSigninAsync_WithInvalidPassword_ReturnsError()
    {
        // Arrange - First create a user
        var signupDto = new SignupDto
        {
            Email = $"signin.invalid.{Guid.NewGuid():N}@test.com",
            Password = "Test@123456",
            FirstName = "Signin",
            LastName = "Invalid"
        };
        await HttpClient.PostAsJsonAsync(SignupUrl, signupDto, CancellationToken);

        var signinDto = new SigninDto
        {
            Email = signupDto.Email,
            Password = "WrongPassword123"
        };

        // Act
        using var response = await HttpClient.PostAsJsonAsync(SigninUrl, signinDto, CancellationToken);

        // Assert
        Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task CreateSigninAsync_WithNonExistentEmail_ReturnsError()
    {
        // Arrange
        var signinDto = new SigninDto
        {
            Email = $"nonexistent.{Guid.NewGuid():N}@test.com",
            Password = "Test@123456"
        };

        // Act
        using var response = await HttpClient.PostAsJsonAsync(SigninUrl, signinDto, CancellationToken);

        // Assert
        Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region Signup Tests

    [TestMethod]
    public async Task CreateSignupAsync_WithValidData_ReturnsUserDto()
    {
        // Arrange
        var signupDto = new SignupDto
        {
            Email = $"signup.valid.{Guid.NewGuid():N}@test.com",
            Password = "Test@123456",
            FirstName = "Signup",
            LastName = "Valid"
        };

        // Act
        using var response = await HttpClient.PostAsJsonAsync(SignupUrl, signupDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<UserDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Id > 0);
        Assert.AreEqual(signupDto.FirstName, result.FirstName);
        Assert.AreEqual(signupDto.LastName, result.LastName);
    }

    [TestMethod]
    public async Task CreateSignupAsync_WithDuplicateEmail_ReturnsError()
    {
        // Arrange - First signup
        var signupDto = new SignupDto
        {
            Email = $"signup.duplicate.{Guid.NewGuid():N}@test.com",
            Password = "Test@123456",
            FirstName = "Signup",
            LastName = "Duplicate"
        };
        await HttpClient.PostAsJsonAsync(SignupUrl, signupDto, CancellationToken);

        // Act - Try to signup with same email
        using var response = await HttpClient.PostAsJsonAsync(SignupUrl, signupDto, CancellationToken);

        // Assert
        Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task CreateSignupAsync_VerifyUserDtoStructure_ContainsAllRequiredProperties()
    {
        // Arrange
        var signupDto = new SignupDto
        {
            Email = $"signup.structure.{Guid.NewGuid():N}@test.com",
            Password = "Test@123456",
            FirstName = "Structure",
            LastName = "Test"
        };

        // Act
        using var response = await HttpClient.PostAsJsonAsync(SignupUrl, signupDto, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<UserDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Id > 0, "Id should be set");
        Assert.IsFalse(string.IsNullOrEmpty(result.FirstName), "FirstName should not be empty");
        Assert.IsFalse(string.IsNullOrEmpty(result.LastName), "LastName should not be empty");
        Assert.IsNotNull(result.Roles, "Roles array should be initialized");
        Assert.IsNotNull(result.Permissions, "Permissions array should be initialized");
    }

    #endregion

    #region Session Tests

    [TestMethod]
    public async Task GetSessionAsync_WithUnauthenticatedUser_ReturnsEmptySession()
    {
        // Arrange
        var client = HttpClient; // Not authenticated

        // Act
        using var response = await client.GetAsync(SessionUrl, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<SessionDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.IsNull(result.User);
        Assert.IsNotNull(result.App);
    }

    [TestMethod]
    public async Task GetSessionAsync_VerifySessionDtoStructure_ContainsAllRequiredProperties()
    {
        // Arrange
        var client = HttpClient;

        // Act
        using var response = await client.GetAsync(SessionUrl, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<SessionDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        
        // Verify App property
        Assert.IsNotNull(result.App);
        Assert.IsFalse(string.IsNullOrEmpty(result.App.Name));
        Assert.IsFalse(string.IsNullOrEmpty(result.App.Version));
    }

    [TestMethod]
    public async Task GetSessionAsync_VerifySessionUserDtoStructure_WhenAuthenticated()
    {
        // Arrange - First create a user to ensure they exist in the database
        var signupDto = new SignupDto
        {
            Email = $"session.authenticated.{Guid.NewGuid():N}@test.com",
            Password = "Test@123456",
            FirstName = "Authenticated",
            LastName = "TestUser"
        };
        using var signupResponse = await HttpClient.PostAsJsonAsync(SignupUrl, signupDto, CancellationToken);
        var createdUser = await signupResponse.Content.ReadFromJsonAsync<UserDto>(CancellationToken);
        Assert.IsNotNull(createdUser);

        // Create authenticated client using SessionDto directly for maximum extensibility
        var sessionDto = new SessionDto
        {
            User = new SessionUserDto
            {
                Id = createdUser.Id,
                EmailAddress = signupDto.Email,
                FirstName = signupDto.FirstName,
                LastName = signupDto.LastName,
                Permissions = [Constants.Permissions.Users_Read],
                Roles = []
            }
        };
        var authenticatedClient = CreateAuthenticatedClient(sessionDto);

        // Act
        using var response = await authenticatedClient.GetAsync(SessionUrl, CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<SessionDto>(CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.App);
        Assert.IsNotNull(result.User, "Authenticated user should have User data in session");
        
        // Verify SessionUserDto structure when user is authenticated
        Assert.AreEqual(createdUser.Id, result.User.Id, "User Id should match");
        Assert.AreEqual(signupDto.Email, result.User.EmailAddress, "EmailAddress should match");
        Assert.AreEqual(signupDto.FirstName, result.User.FirstName, "FirstName should match");
        Assert.AreEqual(signupDto.LastName, result.User.LastName, "LastName should match");
        Assert.IsNotNull(result.User.Permissions, "Permissions array should be initialized");
        Assert.IsNotNull(result.User.Roles, "Roles array should be initialized");
    }

    #endregion
}
