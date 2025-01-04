using API.Core.Configuration;
using API.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Application.Interfaces;

public interface IAppDbContext {
    public DbSet<Note> Notes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    void Attach<T>(T entity) where T : class;
}