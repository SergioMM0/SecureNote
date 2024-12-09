using Domain;

namespace Application.Interface;

public interface ITagRepository
{
    public IEnumerable<Tag> GetAll();
}