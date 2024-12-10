using API.Core.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace API.Infrastructure;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid> {
        
}