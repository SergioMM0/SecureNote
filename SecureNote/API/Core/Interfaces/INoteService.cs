using API.Core.Domain.Entities;

namespace API.Core.Interfaces;

public interface INoteService {
    Task<IEnumerable<Note>> GetAllByUserId(Guid userId);
    Task<Note?> Get(Guid id, bool blur = true);
    Task<Note> Create();
    Task<Note> Update(Note note);
    Task Delete(Guid id);
}
