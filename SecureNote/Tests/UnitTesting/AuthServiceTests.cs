using System.Security.Claims;
using API.Controllers;
using API.Core.Domain.DTO.Auth;
using API.Core.Identity.Entities;
using API.Core.Identity.Managers;
using API.Core.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

public class AuthTests {
    private readonly Mock<IUserStore<ApplicationUser>> _mockUserStore;
    private readonly Mock<IOptions<IdentityOptions>> _mockOptions;
    private readonly Mock<IPasswordHasher<ApplicationUser>> _mockPasswordHasher;
    private readonly Mock<ILookupNormalizer> _mockKeyNormalizer;
    private readonly Mock<IdentityErrorDescriber> _mockErrors;
    private readonly Mock<IServiceProvider> _mockServices;
    private readonly Mock<ILogger<UserManager<ApplicationUser>>> _mockLogger;
    private readonly Mock<CustomUserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<CustomSignInManager<ApplicationUser>> _mockSignInManager;
    private readonly Mock<IJwtService> _mockJwtService;
    private readonly AuthController _controller;

    public AuthTests() {
        _mockUserStore = new Mock<IUserStore<ApplicationUser>>();
        _mockOptions = new Mock<IOptions<IdentityOptions>>();
        _mockPasswordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
        _mockKeyNormalizer = new Mock<ILookupNormalizer>();
        _mockErrors = new Mock<IdentityErrorDescriber>();
        _mockServices = new Mock<IServiceProvider>();
        _mockLogger = new Mock<ILogger<UserManager<ApplicationUser>>>();

        _mockUserManager = new Mock<CustomUserManager<ApplicationUser>>(
            _mockUserStore.Object,
            _mockOptions.Object,
            _mockPasswordHasher.Object,
            new List<IUserValidator<ApplicationUser>>(),
            new List<IPasswordValidator<ApplicationUser>>(),
            _mockKeyNormalizer.Object,
            _mockErrors.Object,
            _mockServices.Object,
            _mockLogger.Object,
            null // here goes AppDbContext, which is not needed for the unit tests (null should be fine)
        );

        _mockSignInManager = new Mock<CustomSignInManager<ApplicationUser>>(
            _mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
            _mockOptions.Object,
            new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<ApplicationUser>>().Object,
            new Mock<IJwtService>().Object
        );

        _mockJwtService = new Mock<IJwtService>();
        _controller = new AuthController(_mockUserManager.Object, _mockSignInManager.Object, _mockJwtService.Object);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsJwtToken() {
        // Arrange
        var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "test@example.com", UserName = "testuser", IsActive = true };
        var roles = new[] { "User" };

        _mockUserManager.Setup(um => um.FindByEmailAsync("test@example.com")).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(roles);

        _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(user, "password", false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        _mockJwtService.Setup(js => js.GenerateJwtToken(user, roles, null)).Returns("ValidJwtToken");

        var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };

        // Act
        var result = await _controller.LogIn(loginDto) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeOfType<LoginSuccessDto>();
        var loginSuccess = result.Value as LoginSuccessDto;
        loginSuccess!.Token.Should().Be("ValidJwtToken");
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsBadRequest() {
        // Arrange
        _mockUserManager.Setup(um => um.FindByEmailAsync("test@example.com")).ReturnsAsync((ApplicationUser)null);

        var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };

        // Act
        var result = await _controller.LogIn(loginDto) as BadRequestObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().Be("Invalid email or password (user inactive)");
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsJwtToken() {
        // Arrange
        var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "test@example.com", UserName = "testuser" };
        var roles = new[] { "User" };

        _mockUserManager.Setup(um => um.Register(It.IsAny<ApplicationUser>(), "password"))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<ApplicationUser, string>((u, _) => {
                u.Id = user.Id;
                u.Email = user.Email;
                u.UserName = user.UserName;
            });

        _mockUserManager.Setup(um => um.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)))
            .ReturnsAsync(roles);

        _mockJwtService.Setup(js => js.GenerateJwtToken(It.Is<ApplicationUser>(u => u.Id == user.Id), roles, null))
            .Returns("ValidJwtToken");

        var registerDto = new RegisterDto { Email = "test@example.com", Username = "testuser", Password = "password" };

        // Act
        var result = await _controller.Register(registerDto) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeOfType<LoginSuccessDto>();
        var loginSuccess = result.Value as LoginSuccessDto;
        loginSuccess!.Token.Should().Be("ValidJwtToken", "because a valid token should be generated upon successful registration");
    }

    [Fact]
    public async Task VerifyToken_WithValidToken_ReturnsOk() {
        // Arrange
        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Role, "User")
        }));

        _mockJwtService.Setup(js => js.ValidateJwtToken("ValidJwtToken")).Returns(claims);

        // Act
        var result = _controller.VerifyToken("Bearer ValidJwtToken") as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().NotBeNull();
        var response = result.Value;
        var messageProperty = response!.GetType().GetProperty("message");
        messageProperty.Should().NotBeNull();
        var messageValue = messageProperty!.GetValue(response) as string;
        messageValue.Should().Be("Token is valid");
    }

    [Fact]
    public async Task VerifyToken_WithInvalidToken_ReturnsUnauthorized() {
        // Arrange
        _mockJwtService.Setup(js => js.ValidateJwtToken("InvalidJwtToken")).Returns((ClaimsPrincipal)null);

        // Act
        var result = _controller.VerifyToken("Bearer InvalidJwtToken") as UnauthorizedObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().NotBeNull();
        var messageProperty = result.Value!.GetType().GetProperty("message");
        messageProperty.Should().NotBeNull();
        var messageValue = messageProperty!.GetValue(result.Value) as string;
        messageValue.Should().Be("Invalid or expired token");
    }
}
