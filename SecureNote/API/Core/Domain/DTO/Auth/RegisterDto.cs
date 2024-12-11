using FluentValidation;

namespace API.Core.Domain.DTO.Auth;

public class RegisterDto {
    /// <summary>
    /// The user's first name.
    /// </summary>
    /// <example>John</example>
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// The user's last name.
    /// </summary>
    /// <example>Doe</example>
    public string LastName { get; set; } = default!;

    /// <summary>
    /// The user's email address.
    /// </summary>
    /// <example>john.doe@example</example>
    public string Email { get; set; } = default!;

    /// <summary>
    /// The user's phone number.
    /// </summary>
    /// <example>+45 12 34 56 78</example>
    public string? PhoneNumber { get; set; } = default!;

    /// <summary>
    /// The user's password.
    /// </summary>
    /// <example>Passw0rd!</example>
    public string Password { get; set; } = default!;    
}

public class RegisterDtoValidator : AbstractValidator<RegisterDto> {
    public RegisterDtoValidator() {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}