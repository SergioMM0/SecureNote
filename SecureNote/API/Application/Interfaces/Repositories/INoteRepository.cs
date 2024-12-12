using API.Core.Domain.Entities;

namespace API.Application.Interfaces.Repositories;

public interface INoteRepository {
    Task<IEnumerable<Note>> GetAllByUserId(Guid userId, bool nfsw);
    Task<Note?> Get(Guid id);
    Task<Note> Create(Note note);
    void Delete(Guid id);
}
