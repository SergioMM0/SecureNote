using API.Core.Domain.Entities;

namespace API.Core.Interfaces;

public interface INoteService {
    Task<IEnumerable<Note>> GetAllFromUser(Guid userId, bool nfsw);
    Task<Note?> Get(Guid id);
    Task<Note> Create();
    Task<Note> Update(Note note);
    Task Delete(Guid id);
}
