using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using uga_mpl_server.Data;
using uga_mpl_server.DTO.User;
using uga_mpl_server.RequestHelpers;

namespace uga_mpl_server;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ApplicationDBContext db, IConfiguration config, IMapper mapper) : ControllerBase
{
    // http://localhost:5274/api/auth/google-signin
    [HttpPost("google-signin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GoogleSignIn()
    {
        // If we reach here, the token is already validated by middleware.
        // Extract claims the middleware parsed from the Google ID token.
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Google 'sub'
        var email = User.FindFirstValue(ClaimTypes.Email);
        var name = User.FindFirstValue("name");

        // %%%%%%%%%%%%%%%% VALIDATE UGA EMAIL DOMAIN %%%%%%%%%%%%%%%%
        if (string.IsNullOrEmpty(email) || !email.EndsWith("@uga.edu", StringComparison.OrdinalIgnoreCase))
            return Unauthorized(new { message = "Only UGA email addresses are permitted." });

        var dbUser = await db.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (dbUser == null)
            return NotFound(new { message = "Email not found. Please sign up first." });

        var token = AuthHelper.GetUserToken(dbUser, config);

        return Ok(new
        {
            message = "Authenticated",
            googleId = userId,
            email,
            name,
            token,
            user = mapper.Map<UserDTO>(dbUser)
        });
    }

    // http://localhost:5274/api/auth/google-signup
    [HttpPost("google-signup")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GoogleSignUp()
    {
        // If we reach here, the token is already validated by middleware.
        // Extract claims the middleware parsed from the Google ID token.
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Google 'sub'
        var email = User.FindFirstValue(ClaimTypes.Email);
        var name = User.FindFirstValue("name");

        var exists = await db.Users.AnyAsync(u => u.Email == email);
        if (exists)
            return Conflict(new { message = "An account with this email already exists." });

        return Ok(new
        {
            message = "Authenticated",
            googleId = userId,
            email,
            name
        });
    }

    // http://localhost:5274/api/auth/create-account
    [HttpPost("create-account")]
    public async Task<IActionResult> CreateAccount(CreateUserDTO createUserDTO)
    {
        // %%%%%%%%%%%%%%%% VALIDATE UGA EMAIL DOMAIN %%%%%%%%%%%%%%%%
        if (!createUserDTO.Email.EndsWith("@uga.edu", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { message = "Only UGA email addresses are permitted." });

        var exists = await db.Users.AnyAsync(u => u.Email == createUserDTO.Email);
        if (exists)
            return Conflict(new { message = "An account with this email already exists." });

        var user = mapper.Map<Entities.User>(createUserDTO);
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var token = AuthHelper.GetUserToken(user, config);

        return Ok(new
        {
            token,
            user = mapper.Map<UserDTO>(user)
        });
    }
}
