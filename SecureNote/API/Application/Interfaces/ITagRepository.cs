using Domain;

namespace API.Application.Interfaces;

public interface ITagRepository {
    public IEnumerable<Tag> GetAll();
}
