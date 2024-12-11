using API.Core.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Core.Identity.Managers;

public class CustomUserManager<TUser> : UserManager<TUser> where TUser : ApplicationUser {
    public CustomUserManager(
        IUserStore<TUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<TUser> passwordHasher,
        IEnumerable<IUserValidator<TUser>> userValidators,
        IEnumerable<IPasswordValidator<TUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<TUser>> logger)
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
    }
    
    public virtual async Task<IdentityResult> SetTwoFactorEnabledAndSendEmailAsync(TUser user, bool enabled) {
        var result = await base.SetTwoFactorEnabledAsync(user, enabled);
        return result;
    }

    public virtual async Task<IdentityResult> Register(TUser user, string password) {
        var result = await base.CreateAsync(user, password);
        return result;
    }
}
