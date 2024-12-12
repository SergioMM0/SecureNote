using API.Core.Identity.Entities;
using API.Core.Identity.Managers;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid> {
    public DbSet<Note> Notes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {
    }
}