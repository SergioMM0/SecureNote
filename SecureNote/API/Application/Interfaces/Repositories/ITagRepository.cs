using API.Core.Domain.Entities;

namespace API.Application.Interfaces.Repositories;

public interface ITagRepository {
    Task<IEnumerable<Tag>> GetAll();
}
