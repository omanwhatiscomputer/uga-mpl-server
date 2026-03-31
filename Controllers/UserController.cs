using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using uga_mpl_server.DTO.User;
using uga_mpl_server.Entities;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public UserController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO dto)
    {
        try
        {
            var user = _mapper.Map<User>(dto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var userDto = _mapper.Map<UserDTO>(user);
            return Ok(userDto);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return BadRequest(new { error = "Email already exists" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUser(Guid id)
    {
        return null;
    }

    [HttpGet]
    public async Task<IEnumerable<UserDTO>> GetUsers()
    {
        return null;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDTO dto)
    { return null; }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    { return null; }
}