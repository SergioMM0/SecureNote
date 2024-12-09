namespace Domain.Interfaces;

public interface INoteService
{
    public Note Create();
    public Note Get(Guid id);
    public Note Update(Note note);
    public void Delete(Guid id);
    public string[] Tag(Note note);
}