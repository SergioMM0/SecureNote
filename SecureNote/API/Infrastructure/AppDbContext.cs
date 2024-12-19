using API.Application.Interfaces;
using API.Core.Domain.Entities;
using API.Core.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IAppDbContext{
    public DbSet<Note> Notes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {
    }
    
    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        // Configure one-to-many relationship between ApplicationUser and Note
        builder.Entity<Note>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notes)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    public void Attach<T>(T entity) where T : class {
        throw new NotImplementedException();
    }
}