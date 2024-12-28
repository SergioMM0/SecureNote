using API.Application.Interfaces;
using API.Core.Configuration;
using API.Core.Domain.Entities;
using API.Core.Identity.Entities;
using API.Infrastructure.Converters;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Infrastructure;


public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>{
    public DbSet<Note> Notes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    private readonly IOptions<EncryptionSettings> _encryptionSettings;
    
    public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<EncryptionSettings> encryptionSettings)
        : base(options) {
        _encryptionSettings = encryptionSettings;
    }
    
    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        // Configure one-to-many relationship between ApplicationUser and Note
        builder.Entity<Note>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notes)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Note>()
            .Property(n => n.Content)
            .HasConversion(new EncryptedValueConverter(_encryptionSettings.Value.MasterKey)!);
    }
    /*
    public void Attach<T>(T entity) where T : class {
        throw new NotImplementedException();
    }
    */
}