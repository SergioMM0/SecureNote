using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Core.Configuration;
using API.Core.Identity.Entities;
using API.Core.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Application.Services;

public class JwtService : IJwtService {
    private readonly IOptions<JwtSettings> _jwtSettings;
    private readonly IOptions<MfaSettings> _mfaSettings;

    public JwtService(
        IOptions<JwtSettings> jwtSettings,
        IOptions<MfaSettings> mfaSettings) {
        _jwtSettings = jwtSettings;
        _mfaSettings = mfaSettings;
    }

    public string GenerateJwtToken(ApplicationUser user, IEnumerable<string> roles, IDictionary<string, dynamic>? customClaims) {
        var claims = new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!)
        };
        if (customClaims is not null) {
            claims.AddRange(customClaims.Select(c => new Claim(c.Key, c.Value)));
        }

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.Now.AddMinutes(_jwtSettings.Value.ExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Value.Issuer,
            audience: _jwtSettings.Value.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateMfaChallengeToken(Guid userId) {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_mfaSettings.Value.MfaChallengeKey));
        var token = new JwtSecurityToken(
            audience: "2FA",
            claims: new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) },
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateMfaChallengeToken(string challengeToken, out Guid userId) {
        var param = new TokenValidationParameters {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_mfaSettings.Value.MfaChallengeKey)),
            ClockSkew = TimeSpan.Zero
        };

        try {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(challengeToken, param, out var token);
            if (token is not null) {
                var jwt = tokenHandler.ReadJwtToken(challengeToken);
                var id = jwt.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                userId = Guid.Parse(id);
                return true;
            }
        } catch (Exception e) {
            userId = default;
            return false;
        }

        userId = default;
        return false;
    }
    
    public ClaimsPrincipal? ValidateJwtToken(string token) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
        var validationParameters = new TokenValidationParameters {
            ValidateIssuer = true,  // Ensure issuer is validated
            ValidateAudience = true,  // Ensure audience is validated
            ValidateLifetime = true,  // Ensure token expiration is checked
            ValidateIssuerSigningKey = true,  // Ensure the signing key is validated
            ValidIssuer = _jwtSettings.Value.Issuer,  // Valid issuer from settings
            ValidAudience = _jwtSettings.Value.Audience,  // Valid audience from settings
            IssuerSigningKey = key,  // The key used to sign the token
            ClockSkew = TimeSpan.Zero  // No clock skew (for precise expiration time matching)
        };

        try {
            // Validate the token and return the ClaimsPrincipal if valid
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            Console.WriteLine(principal);
            return principal;
        } catch {
            // Return null if token validation fails
            return null;
        }
    }

}