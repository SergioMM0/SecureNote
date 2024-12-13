using API.Core.Identity.Entities;
using API.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Core.Identity.Managers;

public class CustomUserManager<TUser> : UserManager<TUser> where TUser : ApplicationUser {
    private readonly AppDbContext _context;
    public CustomUserManager(
        IUserStore<TUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<TUser> passwordHasher,
        IEnumerable<IUserValidator<TUser>> userValidators,
        IEnumerable<IPasswordValidator<TUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<TUser>> logger,
        AppDbContext context)
        : base(
            store,
            optionsAccessor,
            passwordHasher,
            userValidators,
            passwordValidators,
            keyNormalizer,
            errors,
            services,
            logger) {
        _context = context;
    }
    
    public virtual async Task<IdentityResult> SetTwoFactorEnabledAndSendEmailAsync(TUser user, bool enabled) {
        var result = await base.SetTwoFactorEnabledAsync(user, enabled);
        return result;
    }

    public virtual async Task<IdentityResult> Register(TUser user, string password) {
        var result = await base.CreateAsync(user, password);
        if (!result.Succeeded) {
            return result;
        }

        await _context.SaveChangesAsync();
        await AddToRoleAsync(user, "User");
        
        return result;
    }
}
