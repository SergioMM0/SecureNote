using API.Core.Domain.Entities;
using Dynamitey;
using Microsoft.AspNetCore.Identity;

namespace API.Core.Identity.Entities;

public class ApplicationUser : IdentityUser<Guid> {
    public Guid Id { get; set; }
    /// <summary>
    /// First name of the user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Last name of the user.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// List of note IDs associated with the user. Used as a navigation property.
    /// </summary>
    public ICollection<Note> Notes { get; set; } = new List<Note>();

    /// <summary>
    /// Whether the user is active or not.
    /// </summary>
    public bool IsActive { get; set; }
}