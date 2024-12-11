using FluentValidation;

namespace API.Core.Domain.DTO.Auth;

public class LoginDto {
    /// <summary>
    /// The email of the user trying to login.
    /// </summary>
    /// <example>user@example.dk</example>
    public string Email { get; set; } = null!;

    /// <summary>
    /// The password of the user trying to login.
    /// </summary>
    /// <example>Passw0rd!</example>
    public string Password { get; set; } = null!;
}

public class LoginDtoValidator : AbstractValidator<LoginDto> {
    public LoginDtoValidator() {
        RuleFor(login => login.Email).NotEmpty().EmailAddress();
        RuleFor(login => login.Password).NotEmpty();
    }
}
