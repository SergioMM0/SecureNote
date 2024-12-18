using System.Net.Mime;
using API.Core.Domain.DTO.Auth;
using API.Core.Identity.Entities;
using API.Core.Identity.Managers;
using API.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase {
    private readonly CustomUserManager<ApplicationUser> _userManager;
    private readonly CustomSignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;
    
    public AuthController(
        CustomUserManager<ApplicationUser> userManager,
        CustomSignInManager<ApplicationUser> signInManager,
        IJwtService jwtService) {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }
    
    [AllowAnonymous]
    [HttpPost("Login")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginSuccessDto))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(RequireTwoFactorDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LogIn([FromBody] LoginDto loginDto) {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user is null || user.IsActive is false) {
            return BadRequest("Invalid email or password (user inactive)");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (result.RequiresTwoFactor) {
            var challenge = _signInManager.GenerateTwoFactorChallenge(user);
            if (challenge is not null) {
                return StatusCode(StatusCodes.Status403Forbidden, new RequireTwoFactorDto(challenge));
            }
        }

        if (result.Succeeded && !user.TwoFactorEnabled) {
            return Ok(new LoginSuccessDto(
                user.Id,
                user.Email!,
                user.UserName!,
                roles,
                _jwtService.GenerateJwtToken(user, roles)
            ));
        }

        return BadRequest("Invalid email or password");
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginSuccessDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto) {
        var user = new ApplicationUser() {
            Email = dto.Email,
            UserName = dto.Username,
            EmailConfirmed = true,
            IsActive = true
        };
        
        var result = await _userManager.Register(user, dto.Password);
        
        var roles = await _userManager.GetRolesAsync(user);
        
        if (result.Succeeded) {
            return Ok(new LoginSuccessDto(
                user.Id,
                user.Email,
                user.UserName,
                roles,
                _jwtService.GenerateJwtToken(user, roles)
            ));
        }
        return BadRequest(result.Errors);
    }
}