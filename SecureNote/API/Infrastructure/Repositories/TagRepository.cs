using API.Application.Interfaces.Repositories;
using API.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Repositories;

public class TagRepository : ITagRepository {
    private readonly AppDbContext _dbContext;
    
    public TagRepository(AppDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Tag>> GetAll() {
        return await _dbContext.Tags.ToListAsync();
    }
}