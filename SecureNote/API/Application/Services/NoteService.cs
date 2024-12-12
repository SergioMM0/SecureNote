using API.Application.Interfaces.Repositories;
using API.Core.Domain.DTO.User;
using API.Core.Domain.Entities;
using API.Core.Identity.Entities;
using API.Core.Interfaces;

namespace API.Application.Services;

public class NoteService : INoteService {
    private readonly INoteRepository _noteRepository;
    private readonly ITagRepository _tagRepository;

    // When the user presses the "New Note" button, the Create method is called, an empty note is created, and the note is returned.
    public Note Create() {
        return _noteRepository.Create(new Note() {
            UserId = new Guid(),
            User = null!
        });
    }

    // When the user presses the "Save" button, the Update method is called, the note and it's tags are updated, and the note is returned.
    public Note Update(Note note) {
        // Update (rewrite) the tags associated with the note
        note.Tags = Tag(note);

        return _noteRepository.Update(note);
    }

    public void Delete(Guid id) {
        _noteRepository.Delete(id);
    }

    public Note Get(Guid id) {
        return _noteRepository.Get(id);
    }
    
    /// <summary>
    /// Analyzes the provided note and assigns relevant tags based on the content and title.
    /// </summary>
    /// <param name="note">The note object containing the title and content to analyze.</param>
    /// <returns>
    /// An array of strings representing all the matched tags based on the note's title and content.
    /// If no tags match or the note is invalid (has no title AND content), returns an empty array.
    /// </returns>
    public string[] Tag(Note note) {
        // Validate the note's content and title
        if (string.IsNullOrWhiteSpace(note.Title) && string.IsNullOrWhiteSpace(note.Content)) {
            return Array.Empty<string>();
        }

        // Fetch all available tags from the repository
        var tags = _tagRepository.GetAll();

        // Create a set to store matched tags
        var matchedTags = new HashSet<string>();

        // Combine title and content for case-insensitive matching (flipped order)
        var searchableText = $"{note.Title} {note.Content}".ToLower();

        // Iterate through each tag in the repository
        foreach (var tag in tags) {
            // Check if any of the tag's keywords match the combined text
            if (tag.Keywords.Any(keyword => searchableText.Contains(keyword.ToLower()))) {
                matchedTags.Add(tag.Name);
            }
        }

        // Return the matched tags as an array
        return matchedTags.ToArray();
    }
}
