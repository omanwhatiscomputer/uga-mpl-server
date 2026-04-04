using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using uga_mpl_server.Data;
using uga_mpl_server.DTO.Product;
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

        return Ok(await BuildUserDTO(user));
    }

    // GET api/user/{id}
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDTO>> GetUserById(Guid id)
    {
        var user = await db.Users.FindAsync(id);

        if (user == null)
            return NotFound(new { message = "User not found." });

        return Ok(await BuildUserDTO(user));
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
            user = await BuildUserDTO(user)
        });
    }

    // POST api/user/wishlist/{productId}
    [Authorize]
    [HttpPost("wishlist/{productId:guid}")]
    public async Task<IActionResult> AddToWishlist(Guid productId)
    {
        var (user, product, error) = await ResolveUserAndProduct(productId);
        if (error != null) return error;

        if (user!.WishlistedProductIds.Contains(productId))
            return Conflict(new { message = "Product is already in your wishlist." });

        user.WishlistedProductIds.Add(productId);
        product!.WishlistedByUserIds.Add(user.Id);

        await db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE api/user/wishlist/{productId}
    [Authorize]
    [HttpDelete("wishlist/{productId:guid}")]
    public async Task<IActionResult> RemoveFromWishlist(Guid productId)
    {
        var (user, product, error) = await ResolveUserAndProduct(productId);
        if (error != null) return error;

        user!.WishlistedProductIds.Remove(productId);
        product!.WishlistedByUserIds.Remove(user.Id);

        await db.SaveChangesAsync();
        return NoContent();
    }

    // POST api/user/subscribe/{productId}
    [Authorize]
    [HttpPost("subscribe/{productId:guid}")]
    public async Task<IActionResult> Subscribe(Guid productId)
    {
        var (user, product, error) = await ResolveUserAndProduct(productId);
        if (error != null) return error;

        if (user!.SubscribedProductIds.Contains(productId))
            return Conflict(new { message = "Already subscribed to this product." });

        user.SubscribedProductIds.Add(productId);
        product!.SubscriberIds.Add(user.Id);

        await db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE api/user/subscribe/{productId}
    [Authorize]
    [HttpDelete("subscribe/{productId:guid}")]
    public async Task<IActionResult> Unsubscribe(Guid productId)
    {
        var (user, product, error) = await ResolveUserAndProduct(productId);
        if (error != null) return error;

        user!.SubscribedProductIds.Remove(productId);
        product!.SubscriberIds.Remove(user.Id);

        await db.SaveChangesAsync();
        return NoContent();
    }

    // --- Helpers ---

    private async Task<UserDTO> BuildUserDTO(User user)
    {
        var dto = mapper.Map<UserDTO>(user);

        var wishlist = await db.Products
            .Where(p => user.WishlistedProductIds.Contains(p.Id))
            .Include(p => p.Seller)
            .ToListAsync();

        var subscriptions = await db.Products
            .Where(p => user.SubscribedProductIds.Contains(p.Id))
            .Include(p => p.Seller)
            .ToListAsync();

        dto.Wishlist = mapper.Map<List<ProductSummaryDTO>>(wishlist);
        dto.Subscriptions = mapper.Map<List<ProductSummaryDTO>>(subscriptions);

        return dto;
    }

    private async Task<(User user, Product product, IActionResult error)> ResolveUserAndProduct(Guid productId)
    {
        var userIdClaim = User.FindFirst("userid")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return (null, null, Unauthorized(new { message = "Invalid token." }));

        var user = await db.Users.FindAsync(userId);
        if (user == null)
            return (null, null, NotFound(new { message = "User not found." }));

        var product = await db.Products.FindAsync(productId);
        if (product == null)
            return (null, null, NotFound(new { message = "Product not found." }));

        return (user, product, null);
    }
}
