namespace API.Core.Domain.DTO.Auth;

public class RequireTwoFactorDto {
    /// <summary>
    /// The type of the requirement.
    /// </summary>
    public string Type => "mfa_required";

    /// <summary>
    /// The challenge for the two-factor authentication.
    /// </summary>
    public string Challenge { get; set; }

    /// <summary>
    /// Initializes a new instance of the RequireTwoFactorDto class.
    /// </summary>
    /// <param name="challenge">The challenge for the two-factor authentication.</param>
    public RequireTwoFactorDto(string challenge) {
        Challenge = challenge;
    }
}
