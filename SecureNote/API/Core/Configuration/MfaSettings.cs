namespace API.Core.Configuration;

/// <summary>
/// Represents the settings for Multi-Factor Authentication (MFA) configuration.
/// </summary>
public class MfaSettings {
    /// <summary>
    /// Gets or sets the key used for MFA challenges.
    /// </summary>
    public string MfaChallengeKey { get; set; } = null!;
}
