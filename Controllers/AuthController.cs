using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace uga_mpl_server;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // http://localhost:5274/api/auth/google-signin
    [HttpPost("google-signin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GoogleSignIn()
    {
        // If we reach here, the token is already validated by middleware.
        // Extract claims the middleware parsed from the Google ID token.
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Google 'sub'
        var email = User.FindFirstValue(ClaimTypes.Email);
        var name = User.FindFirstValue("name");

        // implement checks for user email validation, domain restrictions (only uga.edu)
        // This is where you'd look up or create the user in your DB
        // e.g. var user = await User.FindOrCreate(userId, email, name);
        // Start with creating entities and DTOs. That way it's going to be more easier to maintain.

        return Ok(new
        {
            message = "Authenticated",
            googleId = userId,
            email,
            name
        });
    }
}
