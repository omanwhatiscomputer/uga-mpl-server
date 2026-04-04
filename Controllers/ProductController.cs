using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using uga_mpl_server.Data;
using uga_mpl_server.DTO.Product;
using uga_mpl_server.DTO.User;
using uga_mpl_server.Entities;
using uga_mpl_server.Enums;

namespace uga_mpl_server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController(ApplicationDBContext db, IMapper mapper) : ControllerBase
{
    // POST api/product
    [HttpPost]
    public async Task<ActionResult<ProductDTO>> CreateProduct(CreateProductDTO createProductDTO)
    {
        var sellerIdClaim = User.FindFirst("userid")?.Value;

        if (string.IsNullOrEmpty(sellerIdClaim) || !Guid.TryParse(sellerIdClaim, out var sellerId))
            return Unauthorized(new { message = "Invalid token." });

        var seller = await db.Users.FindAsync(sellerId);

        if (seller == null)
            return NotFound(new { message = "Seller account not found." });

        var product = mapper.Map<Product>(createProductDTO);
        product.SellerId = sellerId;
        product.Seller = seller;

        db.Products.Add(product);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateProduct), await BuildProductDTO(product));
    }

    // GET api/product
    [HttpGet]
    public async Task<ActionResult<List<ProductSummaryDTO>>> GetAllProducts()
    {
        var products = await db.Products
            .Include(p => p.Seller)
            .ToListAsync();

        return Ok(mapper.Map<List<ProductSummaryDTO>>(products));
    }

    // GET api/product/category/{category}
    [HttpGet("category/{category}")]
    public async Task<ActionResult<List<ProductSummaryDTO>>> GetProductsByCategory(string category)
    {
        if (!Enum.TryParse<Enums.Category>(category, ignoreCase: true, out var parsedCategory))
            return BadRequest(new { message = $"Invalid category '{category}'." });

        var products = await db.Products
            .Where(p => p.Category == parsedCategory)
            .Include(p => p.Seller)
            .ToListAsync();

        return Ok(mapper.Map<List<ProductSummaryDTO>>(products));
    }

    // GET api/product/{id}/subscribers
    [HttpGet("{id:guid}/subscribers")]
    public async Task<ActionResult<List<UserSummaryDTO>>> GetSubscribers(Guid id)
    {
        var sellerIdClaim = User.FindFirst("userid")?.Value;

        if (string.IsNullOrEmpty(sellerIdClaim) || !Guid.TryParse(sellerIdClaim, out var sellerId))
            return Unauthorized(new { message = "Invalid token." });

        var product = await db.Products.FindAsync(id);

        if (product == null)
            return NotFound(new { message = "Product not found." });

        if (product.SellerId != sellerId)
            return Forbid();

        var subscribers = await db.Users
            .Where(u => product.SubscriberIds.Contains(u.Id))
            .ToListAsync();

        return Ok(mapper.Map<List<UserSummaryDTO>>(subscribers));
    }

    // --- Helpers ---

    private async Task<ProductDTO> BuildProductDTO(Product product)
    {
        var dto = mapper.Map<ProductDTO>(product);

        var subscribers = await db.Users
            .Where(u => product.SubscriberIds.Contains(u.Id))
            .ToListAsync();

        var wishlistedBy = await db.Users
            .Where(u => product.WishlistedByUserIds.Contains(u.Id))
            .ToListAsync();

        dto.Subscribers = mapper.Map<List<UserSummaryDTO>>(subscribers);
        dto.WishlistedBy = mapper.Map<List<UserSummaryDTO>>(wishlistedBy);

        return dto;
    }
}
