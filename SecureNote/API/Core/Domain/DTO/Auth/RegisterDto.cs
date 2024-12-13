using FluentValidation;

namespace API.Core.Domain.DTO.Auth;

public class RegisterDto {
    /// <summary>
    /// The user's username.
    /// </summary>
    /// <example>johndoe123</example>
    public string Username { get; set; } = default!;

    /// <summary>
    /// The user's email address.
    /// </summary>
    /// <example>john.doe@example.com</example>
    public string Email { get; set; } = default!;

    /// <summary>
    /// The user's password.
    /// </summary>
    /// <example>Passw0rd!</example>
    public string Password { get; set; } = default!;    
}

public class RegisterDtoValidator : AbstractValidator<RegisterDto> {
    public RegisterDtoValidator() {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email address is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}