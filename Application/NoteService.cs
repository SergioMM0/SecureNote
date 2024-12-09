using Application.Interface;
using Domain;
using Domain.Interfaces;

namespace Application;

public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly ITagRepository _tagRepository;
    
    public Note Create()
    {
        throw new System.NotImplementedException();
    }

    public void Delete(Guid id)
    {
        throw new System.NotImplementedException();
    }

    public Note Get(Guid id)
    {
        throw new System.NotImplementedException();
    }

    public string[] Tag(Note note)
    {
        if (string.IsNullOrWhiteSpace(note.Content))
        {
            throw new ArgumentException("Note or note content cannot be null or empty.");
        }

        // Fetch all available tags from the repository
        var tags = _tagRepository.GetAll();

        // Create a set to store matched tags
        var matchedTags = new HashSet<string>();

        // Lowercase the note content for case-insensitive matching
        var content = note.Content.ToLower();

        // Iterate through each tag in the repository
        foreach (var tag in tags)
        {
            // Check if any of the tag's keywords match the note content
            if (tag.Keywords.Any(keyword => content.Contains(keyword.ToLower())))
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