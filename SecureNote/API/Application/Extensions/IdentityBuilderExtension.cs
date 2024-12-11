using API.Core.Identity.Entities;
using API.Core.Identity.Managers;
using API.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace API.Application.Extensions;

public static class IdentityBuilderExtension {

    public static void BuildIdentityUser(this IServiceCollection services) {
        services.AddIdentityCore<ApplicationUser>(opt => {
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;

                opt.SignIn.RequireConfirmedEmail = true;

                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            })
            .AddRoles<ApplicationRole>()
            .AddUserManager<CustomUserManager<ApplicationUser>>()
            .AddSignInManager<CustomSignInManager<ApplicationUser>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<DataProtectionTokenProviderOptions>(options => {
            options.TokenLifespan = TimeSpan.FromDays(1);
        });
    }
}
