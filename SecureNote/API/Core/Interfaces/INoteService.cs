using API.Core.Domain.Entities;

namespace API.Core.Interfaces;

public interface INoteService {
    Task<IEnumerable<Note>> GetAllFromUser(Guid userId, bool nfsw);
    Task<Note?> Get(Guid id);
    Task Create();
    Task<Note> Update(Note note);
    void Delete(Guid id);
}
