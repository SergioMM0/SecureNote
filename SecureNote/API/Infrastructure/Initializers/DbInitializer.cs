using API.Core.Identity.Entities;
using API.Core.Identity.Managers;
using Microsoft.AspNetCore.Identity;

namespace API.Infrastructure.Initializers;

public class DbInitializer {
    private readonly AppDbContext _db;
    private readonly CustomUserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    
    public DbInitializer(AppDbContext db, CustomUserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager) {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    // seeds the database with initial data
    public async Task Init() {
        await _db.Database.EnsureDeletedAsync();
        await _db.Database.EnsureCreatedAsync();
        
    }
}