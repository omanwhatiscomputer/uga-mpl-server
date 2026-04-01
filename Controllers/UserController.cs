using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using uga_mpl_server.Data;
using uga_mpl_server.DTO.User;
using uga_mpl_server.Entities;
using uga_mpl_server.RequestHelpers;

namespace uga_mpl_server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(ApplicationDBContext db, IConfiguration config, IMapper mapper) : ControllerBase
{
    // GET api/user/by-email?email=...
    // Used by the client after Google SSO to check if a user account exists.
    [Authorize]
    [HttpGet("by-email")]
    public async Task<ActionResult<UserDTO>> GetUserByEmail([FromQuery] string email)
    {
        if (string.IsNullOrEmpty(email))
            return BadRequest(new { message = "Email is required." });

        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return NotFound(new { message = "Email not found." });

        return Ok(mapper.Map<UserDTO>(user));
    }

    // GET api/user/{id}
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDTO>> GetUserById(Guid id)
    {
        var user = await db.Users.FindAsync(id);

        if (user == null)
            return NotFound(new { message = "User not found." });

        return Ok(mapper.Map<UserDTO>(user));
    }

    // POST api/user
    // Sign-up: creates a new user account and issues a JWT cookie.
    [HttpPost]
    public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO createUserDTO)
    {
        if (!createUserDTO.Email.EndsWith("@uga.edu", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { message = "Only UGA email addresses are permitted." });

        var exists = await db.Users.AnyAsync(u => u.Email == createUserDTO.Email);
        if (exists)
            return Conflict(new { message = "An account with this email already exists." });

        var user = mapper.Map<User>(createUserDTO);
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var token = AuthHelper.GetUserToken(user, config);

        return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email }, new
        {
            token,
            user = mapper.Map<UserDTO>(user)
        });
    }
}
