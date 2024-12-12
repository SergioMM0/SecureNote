namespace API.Core.Domain.DTO.User;

// unused for now
public class UserDto {
    /// <summary>
    /// The Id of the user.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The email address of the user.
    /// </summary>
    /// <example>john.doe@example</example>
    public required string Email { get; set; }

    /// <summary>
    /// The user's first name.
    /// </summary>
    /// <example>John</example>
    public required string FirstName { get; set; }

    /// <summary>
    /// The user's last name.
    /// </summary>
    /// <example>Doe</example>
    public required string LastName { get; set; }

    /// <summary>
    /// The user's phone number.
    /// </summary>
    /// <example>+45 12 34 56 78</example>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// A list of the user's roles.
    /// </summary>
    /// <example>
    /// [
    ///     "User"
    /// ]
    /// </example>
    public IList<string>? Roles { get; set; }
    
    /// <summary>
    /// A boolean value indicating whether the user has two-factor authentication enabled.
    /// </summary>
    /// <example>true</example>
    public required bool TwoFactorEnabled { get; set; }
}