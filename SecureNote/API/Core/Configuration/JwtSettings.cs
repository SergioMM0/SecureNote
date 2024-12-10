namespace API.Core.Configuration;

/// <summary>
/// Represents the settings for JSON Web Token (JWT) configuration.
/// </summary>
public class JwtSettings {
    /// <summary>
    /// Gets or sets the security key for the JWT.
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// Gets or sets the issuer of the JWT. It identifies the principal that issued the JWT.
    /// </summary>
    public string Issuer { get; set; } = null!;

    /// <summary>
    /// Gets or sets the audience of the JWT. It identifies the recipients that the JWT is intended for.
    /// </summary>
    public string Audience { get; set; } = null!;

    /// <summary>
    /// Gets or sets the expiration time of the JWT in minutes.
    /// </summary>
    public int ExpirationMinutes { get; set; }
}
