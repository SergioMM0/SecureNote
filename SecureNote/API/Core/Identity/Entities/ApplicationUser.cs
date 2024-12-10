using Microsoft.AspNetCore.Identity;

namespace API.Core.Identity.Entities;

public class ApplicationUser : IdentityUser<Guid> {
    /// <summary>
    /// First name of the user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Last name of the user.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Whether the user is active or not.
    /// </summary>
    public bool IsActive { get; set; }
}