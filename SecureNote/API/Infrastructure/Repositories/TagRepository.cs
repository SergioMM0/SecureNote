using API.Application.Interfaces.Repositories;
using Domain;

namespace API.Infrastructure.Repositories;

public class TagRepository : ITagRepository {

    public IEnumerable<Tag> GetAll() {
        throw new NotImplementedException();
    }
}