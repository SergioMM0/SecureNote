namespace API.Core.Domain.DTO.Auth;

public class LoginSuccessDto {
    /// <summary>
    /// The unique identifier of the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The email of the user.
    /// </summary>
    /// <example>user@example.dk</example>
    public string Email { get; set; }

    /// <summary>
    /// The username of the user logging in.
    /// </summary>
    /// <example>john.doe</example>
    public string Username { get; set; }

    /// <summary>
    /// The roles of the user (User, Admin).
    /// </summary>
    /// <example>["User"]</example>
    public IList<string>? Roles { get; set; }

    /// <summary>
    /// The token of the user.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// The authentication token type.
    /// </summary>
    public string TokenType => "Bearer";

    /// <summary>
    /// Initializes a new instance of the LoginSuccessDto class.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="email">The email of the user.</param>
    /// <param name="username">The username of the user.</param>
    /// <param name="roles">The roles of the user.</param>
    /// <param name="token">The token of the user.</param>
    public LoginSuccessDto(Guid id, string email, string username, IList<string>? roles, string token) {
        Id = id;
        Email = email;
        Username = username;
        Roles = roles;
        Token = token;
    }
}