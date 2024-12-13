using API.Core.Domain.Entities;
using Dynamitey;
using Microsoft.AspNetCore.Identity;

namespace API.Core.Identity.Entities;

public class ApplicationUser : IdentityUser<Guid> {
    /// <summary>
    /// List of note IDs associated with the user. Used as a navigation property.
    /// </summary>
    public ICollection<Note> Notes { get; set; } = new List<Note>();

    /// <summary>
    /// Whether the user is active or not.
    /// </summary>
    public bool IsActive { get; set; }
}