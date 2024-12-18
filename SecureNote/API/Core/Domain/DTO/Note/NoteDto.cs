namespace API.Core.Domain.DTO.Note;

public class NoteDto {
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string[]? Tags { get; set; } = [];
    public bool Nsfw { get; set; }
}