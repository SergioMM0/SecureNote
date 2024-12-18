using API.Core.Identity.Entities;

namespace API.Core.Domain.Entities {
    /// <summary>
    /// Represents a note created by a user, containing a title, content, and optional associated tags.
    /// </summary>
    public class Note {
        /// <summary>
        /// A GUID that uniquely identifies the note.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// A GUID representing the user who owns the note. This property is required. Used as a foreign key.
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// This property is required by EF.
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// A short and descriptive title for the note. This property is required.
        /// </summary>
        /// <example>"Meeting Notes"</example>
        public string? Title { get; set; }

        /// <summary>
        /// The main body of the note, containing detailed information. This property is required.
        /// </summary>
        /// <example>"Discussed quarterly goals and team progress."</example>
        public string? Content { get; set; }
        
        /// <summary>
        /// A boolean value indicating whether the note is safe for work.
        /// </summary>
        public bool Nsfw { get; set; }
        
        /// <summary>
        ///  An optional array of strings representing the tags linked to the note. Defaults to an empty array if not specified.
        /// </summary>
        /// <example>
        /// For a meeting note, tags might include:
        /// <code>["work", "meeting", "progress"]</code>
        /// </example>
        public string[]? Tags { get; set; } = [];
    }
}
