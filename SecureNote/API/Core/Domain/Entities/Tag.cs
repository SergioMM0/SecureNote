namespace API.Core.Domain.Entities {
    /// <summary>
    /// Represents a tag entity used for categorization or labeling.
    /// Each tag has a unique identifier, a name, and associated keywords.
    /// </summary>
    public class Tag {
        /// <summary>
        /// A GUID that uniquely identifies the tag.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// A descriptive and concise name for the tag, such as "Home" or "Work".
        /// </summary>
        /// <example>"ToDo"</example>
        public required string Name { get; set; }

        /// <summary>
        /// An array of strings representing keywords that enhance the tag's searchability.
        /// </summary>
        /// <example>
        /// For a tag with the name "Home", keywords might include:
        /// <code>["house", "family", "living room"]</code>
        /// </example>
        public required string[] Keywords { get; set; }
    }
}
