using API.Core.Identity.Entities;

namespace API.Application.Interfaces.Authentication;

public interface IJwtService {
    public string GenerateJwtToken(ApplicationUser user, IEnumerable<string> roles,
        IDictionary<string, dynamic>? customClaims = null);
    
    public string GenerateMfaChallengeToken(Guid userId);
    
    public bool ValidateMfaChallengeToken(string challengeToken, out Guid userId);
}
