using System.Security.Claims;
using API.Core.Identity.Entities;

namespace API.Core.Interfaces;

public interface IJwtService {
    public string GenerateJwtToken(ApplicationUser user, IEnumerable<string> roles,
        IDictionary<string, dynamic>? customClaims = null);
    
    public string GenerateMfaChallengeToken(Guid userId);
    
    public bool ValidateMfaChallengeToken(string challengeToken, out Guid userId);
    
    public ClaimsPrincipal? ValidateJwtToken(string token);
}
