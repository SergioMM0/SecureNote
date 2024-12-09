using Application.DTOs;
using Application.Interface;
using Domain;
using Domain.Interfaces;

namespace Application;

public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly ITagRepository _tagRepository;
    
    public Note Create(PostNoteDTO note)
    {
        // Validate the note's title and content
        if (string.IsNullOrWhiteSpace(note.Title) && string.IsNullOrWhiteSpace(note.Content))
        {
            throw new ArgumentException("Note title and content cannot be null, empty, or whitespace.");
        }

        // Create a new note object
        var newNote = new Note
        {
            Title = note.Title,
            Content = note.Content,
        };
        
        // Identify tags that match the note's content and title
        newNote.Tags = Tag(newNote);

        // Save the note to the repository
        return _noteRepository.Create(newNote);
    }

    public void Delete(Guid id)
    {
        throw new System.NotImplementedException();
    }

    public Note Get(Guid id)
    {
        throw new System.NotImplementedException();
    }
    
    /// <summary>
    /// Identifies and returns a list of tags that match the content or title of the given note.
    /// </summary>
    /// <param name="note">The note object containing the content and title to analyze.</param>
    /// <returns>
    /// An array of strings representing the names of tags that match keywords found in the note's content or title.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the note, its content, or its title is null, empty, or contains only whitespace.
    /// </exception>
    /// <remarks>
    /// This method performs a case-insensitive search to match keywords associated with tags
    /// against the content and title of the provided note. Tags and their keywords are fetched from
    /// the tag repository.
    /// </remarks>
    public string[] Tag(Note note)
    {
        // Validate the note's content and title
        if (string.IsNullOrWhiteSpace(note.Title) && string.IsNullOrWhiteSpace(note.Content))
        {
            throw new ArgumentException("Note title and content cannot be null, empty, or whitespace.");
        }

        // Fetch all available tags from the repository
        var tags = _tagRepository.GetAll();

        // Create a set to store matched tags
        var matchedTags = new HashSet<string>();

        // Combine title and content for case-insensitive matching (flipped order)
        var searchableText = $"{note.Title} {note.Content}".ToLower();

        // Iterate through each tag in the repository
        foreach (var tag in tags)
        {
            // Check if any of the tag's keywords match the combined text
            if (tag.Keywords.Any(keyword => searchableText.Contains(keyword.ToLower())))
            {
                matchedTags.Add(tag.Name);
            }
        }

        // Return the matched tags as an array
        return matchedTags.ToArray();
    }

    public Note Update(Note note)
    {
        throw new System.NotImplementedException();
    }
}