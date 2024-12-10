using API.Application.Interfaces.Authentication;
using API.Core.Identity.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Core.Identity.Managers;

public class CustomSignInManager<TUser> : SignInManager<TUser> where TUser : ApplicationUser {
    private readonly IJwtService _jwtService;

    public CustomSignInManager(
        UserManager<TUser> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<TUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<TUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<TUser> confirmation,
        IJwtService jwtService)
        : base(userManager,
            contextAccessor,
            claimsFactory,
            optionsAccessor,
            logger,
            schemes,
            confirmation) {
        _jwtService = jwtService;
    }

    /// <summary>
    /// Attempts to sign in the specified <paramref name="user"/> and <paramref name="password"/> combination.
    /// </summary>
    /// <param name="user">The user attempting to sign in.</param>
    /// <param name="password">The password provided for the specified <paramref name="user"/>.</param>
    /// <param name="lockoutOnFailure">The flag indicating if the user account should be locked if the sign in fails.</param>
    /// <returns>A Task that represents the sign-in attempt. The task contains the <see cref="SignInResult"/> for the sign-in attempt.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the specified <paramref name="user"/> is null.</exception>
    public override async Task<SignInResult> CheckPasswordSignInAsync(TUser user, string password, bool lockoutOnFailure) {
        if (user == null) {
            throw new ArgumentNullException(nameof(user));
        }

        var error = await PreSignInCheck(user);
        if (error != null) {
            return error;
        }

        if (await UserManager.CheckPasswordAsync(user, password)) {
            var alwaysLockout = AppContext.TryGetSwitch("Microsoft.AspNetCore.Identity.CheckPasswordSignInAlwaysResetLockoutOnSuccess", out var enabled) && enabled;
            // Only reset the lockout when not in quirks mode if either TFA is not enabled or the client is remembered for TFA.
            var mfaEnabled = await IsMfaEnabled(user);

            if (mfaEnabled) {
                return SignInResult.TwoFactorRequired;
            }

            if (alwaysLockout || !mfaEnabled) {
                await ResetLockout(user);
            }

            return SignInResult.Success;
        }
        Logger.LogDebug(new EventId(2, "InvalidPassword"), "User failed to provide the correct password.");

        if (UserManager.SupportsUserLockout && lockoutOnFailure) {
            // If lockout is requested, increment access failed count which might lock out the user
            await UserManager.AccessFailedAsync(user);
            if (await UserManager.IsLockedOutAsync(user)) {
                return await LockedOut(user);
            }
        }
        return SignInResult.Failed;
    }

    /// <summary>
    /// Verifies the provided two-factor authentication code for the specified challenge.
    /// </summary>
    /// <param name="challenge">The challenge token generated at the time of two-factor authentication request.</param>
    /// <param name="code">The two-factor authentication code provided by the user.</param>
    /// <returns>A Task that represents the two-factor sign-in attempt. The task contains the <see cref="SignInResult"/> for the two-factor sign-in attempt and user information if successful.</returns>
    public virtual async Task<(SignInResult, TUser?)> CheckTwoFactorSignInAsync(string challenge, string code) {
        // Validate challenge token
        var valid = _jwtService.ValidateMfaChallengeToken(challenge, out var userId);
        if (!valid) {
            return (SignInResult.Failed, null);
        }

        // Get user
        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null) {
            return (SignInResult.Failed, null);
        }

        // Ensure that a user is allowed to sign in
        var error = await PreSignInCheck(user);
        if (error != null) {
            return (error, null);
        }

        if (await UserManager.VerifyTwoFactorTokenAsync(user, Options.Tokens.AuthenticatorTokenProvider, code)) {
            await ResetLockout(user);
            return (SignInResult.Success, user);
        }

        // If the token is incorrect, record the failure which also may cause the user to be locked out
        if (UserManager.SupportsUserLockout) {
            await UserManager.AccessFailedAsync(user);
        }

        return (SignInResult.Failed, null);
    }

    /// <summary>
    /// Verifies the provided recovery code for the specified challenge.
    /// </summary>
    /// <param name="challenge">The challenge token generated at the time of two-factor authentication request.</param>
    /// <param name="recoveryCode">The recovery code provided by the user.</param>
    /// <returns>A Task that represents the recovery code sign-in attempt. The task contains the <see cref="SignInResult"/> for the recovery code sign-in attempt and user information if successful.</returns>
    public virtual async Task<(SignInResult, TUser?)> CheckTwoFactorRecoveryCodeSignInAsync(string challenge, string recoveryCode) {
        // Validate challenge token
        var valid = _jwtService.ValidateMfaChallengeToken(challenge, out var userId);
        if (!valid) {
            return (SignInResult.Failed, null);
        }

        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null) {
            return (SignInResult.Failed, null);
        }

        // Ensure that a user is allowed to sign in
        var error = await PreSignInCheck(user);
        if (error != null) {
            return (error, null);
        }

        var result = await UserManager.RedeemTwoFactorRecoveryCodeAsync(user, recoveryCode);
        if (result.Succeeded) {
            return (SignInResult.Success, user);
        }

        // If the recovery code is incorrect, record the failure which also may cause the user to be locked out
        if (UserManager.SupportsUserLockout) {
            await UserManager.AccessFailedAsync(user);
        }

        return (SignInResult.Failed, null);
    }

    /// <summary>
    /// Attempts to generate challenge for 2fa login
    /// </summary>
    /// <param name="user">The user attempting to sign in.</param>
    /// <returns>A string containing the challenge token.</returns>
    public virtual string? GenerateTwoFactorChallenge(TUser user) {
        if (!user.TwoFactorEnabled) {
            return null;
        }

        return _jwtService.GenerateMfaChallengeToken(user.Id);
    }

    /// <summary>
    /// Gets whether the user has two-factor authentication enabled.
    /// </summary>
    /// <param name="user">The user to check.</param>
    /// <returns>A Task containing a boolean indicating whether the user has two-factor authentication enabled.</returns>
    private async Task<bool> IsMfaEnabled(TUser user)
        => UserManager.SupportsUserTwoFactor &&
           await UserManager.GetTwoFactorEnabledAsync(user) &&
           (await UserManager.GetValidTwoFactorProvidersAsync(user)).Count > 0;
}
