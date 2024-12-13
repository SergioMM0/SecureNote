using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Application.Interfaces.Authentication;
using API.Application.Services;
using API.Core.Configuration;
using API.Core.Identity.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace Tests.xUnit
{
    public class JwtServiceTests
    {
        private readonly Mock<IOptions<JwtSettings>> _jwtSettingsMock;
        private readonly Mock<IOptions<MfaSettings>> _mfaSettingsMock;
        private readonly JwtService _jwtService;

        public JwtServiceTests()
        {
            _jwtSettingsMock = new Mock<IOptions<JwtSettings>>();
            _mfaSettingsMock = new Mock<IOptions<MfaSettings>>();

            _jwtSettingsMock.Setup(x => x.Value).Returns(new JwtSettings
            {
                Key = "supersecretkey",
                Issuer = "testIssuer",
                Audience = "testAudience",
                ExpirationMinutes = 60
            });

            _mfaSettingsMock.Setup(x => x.Value).Returns(new MfaSettings
            {
                MfaChallengeKey = "mfasecretkey"
            });

            _jwtService = new JwtService(_jwtSettingsMock.Object, _mfaSettingsMock.Object);
        }

        [Fact]
        public void GenerateJwtToken_ReturnsToken()
        {
            // Arrange
            var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "test@example.com", FirstName = "Test" };
            var roles = new List<string> { "Admin" };
            var customClaims = new Dictionary<string, dynamic> { { "CustomClaim", "CustomValue" } };

            // Act
            var token = _jwtService.GenerateJwtToken(user, roles, customClaims);

            // Assert
            Assert.NotNull(token);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            Assert.Equal(user.Id.ToString(), jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Assert.Equal(user.Email, jwtToken.Claims.First(c => c.Type == ClaimTypes.Email).Value);
            Assert.Equal(user.FirstName, jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            Assert.Equal("CustomValue", jwtToken.Claims.First(c => c.Type == "CustomClaim").Value);
            Assert.Equal("Admin", jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value);
        }

        [Fact]
        public void GenerateMfaChallengeToken_ReturnsToken()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var token = _jwtService.GenerateMfaChallengeToken(userId);

            // Assert
            Assert.NotNull(token);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            Assert.Equal(userId.ToString(), jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }

        [Fact]
        public void ValidateMfaChallengeToken_ValidToken_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var token = _jwtService.GenerateMfaChallengeToken(userId);

            // Act
            var result = _jwtService.ValidateMfaChallengeToken(token, out var validatedUserId);

            // Assert
            Assert.True(result);
            Assert.Equal(userId, validatedUserId);
        }

        [Fact]
        public void ValidateMfaChallengeToken_InvalidToken_ReturnsFalse()
        {
            // Arrange
            var invalidToken = "invalidToken";

            // Act
            var result = _jwtService.ValidateMfaChallengeToken(invalidToken, out var validatedUserId);

            // Assert
            Assert.False(result);
            Assert.Equal(Guid.Empty, validatedUserId);
        }
    }
}
