using API.Application.Interfaces.Repositories;
using Domain;

namespace API.Infrastructure.Repositories;

public class NoteRepository : INoteRepository {

    public Note Create(Note note) {
        throw new NotImplementedException();
    }
    public Note Get(Guid id) {
        throw new NotImplementedException();
    }
    public Note Update(Note note) {
        throw new NotImplementedException();
    }
    public void Delete(Guid id) {
        throw new NotImplementedException();
    }
}