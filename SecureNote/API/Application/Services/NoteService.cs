using API.Application.Interfaces.Repositories;
using API.Core.Domain.Context;
using API.Core.Domain.Entities;
using API.Core.Domain.Exception;
using API.Core.Interfaces;
using API.Infrastructure;

namespace API.Application.Services;

public class NoteService : INoteService {
    private readonly INoteRepository _noteRepository;
    private readonly ITagRepository _tagRepository;
    private readonly AppDbContext _dbContext;
    private readonly CurrentContext _currentContext;
    
    public NoteService(INoteRepository noteRepository, ITagRepository tagRepository, AppDbContext dbContext, CurrentContext currentContext) {
        _noteRepository = noteRepository;
        _tagRepository = tagRepository;
        _dbContext = dbContext;
        _currentContext = currentContext;
    }
    
    public Task<IEnumerable<Note>> GetAllByUserId(Guid userId) {
        return _noteRepository.GetAllByUserId(userId);
    }
    
    public async Task<Note?> Get(Guid id) {
        return await _noteRepository.Get(id);
    }
    
    /// <summary>
    /// When the user presses the "New Note" button, the Create method is called, an empty note is created, and the note is returned.
    /// </summary>
    public async Task<Note> Create() {
        var result = await _noteRepository.Create(new Note {
            UserId = (Guid) _currentContext.UserId!
        });
        await _dbContext.SaveChangesAsync();
        return result;
    }

    /// <summary>
    /// When the user presses the "Save" button, the Update method is called, the note and it's tags are updated, and the note is returned.
    /// </summary>
    /// <param name="note"></param>
    /// <returns></returns>
    public async Task<Note> Update(Note note) {
        var result = await _noteRepository.Get(note.Id);
        
        if (result is null) {
            throw new NotFoundException($"Note with id: {note.Id} not found.");
        }
        
        // Update (rewrite) the tags associated with the note
        _dbContext.Attach(result);
        result.Title = note.Title;
        result.Content = note.Content;
        result.Tags = await Tag(result);
        
        await _dbContext.SaveChangesAsync();
        return result;
    }

    public async Task Delete(Guid id) {
        var note = await _noteRepository.Get(id);
        
        if (note is null) {
            throw new NotFoundException($"Note with id: {id} not found.");
        }
        
        _noteRepository.Delete(note);
        await _dbContext.SaveChangesAsync();
    }
    
    /// <summary>
    /// Analyzes the provided note and assigns relevant tags based on the content and title.
    /// </summary>
    /// <param name="note">The note object containing the title and content to analyze.</param>
    /// <returns>
    /// An array of strings representing all the matched tags based on the note's title and content.
    /// If no tags match or the note is invalid (has no title AND content), returns an empty array.
    /// </returns>
    private async Task<string[]> Tag(Note note) {
        // Validate the note's content and title
        if (string.IsNullOrWhiteSpace(note.Title) && string.IsNullOrWhiteSpace(note.Content)) {
            return [];
        }

        // Fetch all available tags from the repository
        var tags = await _tagRepository.GetAll();

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
