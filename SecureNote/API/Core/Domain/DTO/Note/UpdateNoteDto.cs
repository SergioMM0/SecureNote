using FluentValidation;

namespace API.Core.Domain.DTO.Note;

public class UpdateNoteDto {
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public required bool Nfsw { get; set; }
}

public class UpdateNoteDtoValidator : AbstractValidator<UpdateNoteDto> {
    public UpdateNoteDtoValidator() {
        RuleFor(note => note.Id).NotEmpty();
        RuleFor(note => note.Nfsw).NotEmpty();
    }
}