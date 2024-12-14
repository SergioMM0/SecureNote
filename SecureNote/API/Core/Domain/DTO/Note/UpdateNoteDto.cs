using FluentValidation;

namespace API.Core.Domain.DTO.Note;

public class UpdateNoteDto {
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public required bool Nfsw { get; set; }
}

public class UpdateNoteDtoValidator : AbstractValidator<UpdateNoteDto> {
    public UpdateNoteDtoValidator() {
        RuleFor(login => login.Title).NotEmpty();
        RuleFor(login => login.Content).NotEmpty();
    }
}