using Domain;

namespace Application.Interface;

public interface INoteRepository
{
    public Note Create(Note note);
    public Note Get(Guid id);
    public Note Update(Note note);
    public void Delete(Guid id);
}