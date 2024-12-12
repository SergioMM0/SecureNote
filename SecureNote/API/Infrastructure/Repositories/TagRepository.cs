using API.Application.Interfaces.Repositories;
using API.Core.Domain.Entities;

namespace API.Infrastructure.Repositories;

public class TagRepository : ITagRepository {

    public IEnumerable<Tag> GetAll() {
        throw new NotImplementedException();
    }
}