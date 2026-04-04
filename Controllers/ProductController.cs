using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using uga_mpl_server.Data;
using uga_mpl_server.DTO.Product;
using uga_mpl_server.Entities;

namespace uga_mpl_server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController(ApplicationDBContext db, IMapper mapper) : ControllerBase
{
    // GET api/product
    [HttpGet]
    public async Task<ActionResult<List<ProductDTO>>> GetProducts()
    {
        var products = await db.Products
            .Include(p => p.Seller)
            .OrderByDescending(p => p.DateCreated)
            .ToListAsync();

        return Ok(mapper.Map<List<ProductDTO>>(products));
    }

    // GET api/product/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDTO>> GetProductById(Guid id)
    {
        var product = await db.Products
            .Include(p => p.Seller)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound(new { message = "Product not found." });

        return Ok(mapper.Map<ProductDTO>(product));
    }

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

        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, mapper.Map<ProductDTO>(product));
    }

    // PATCH api/product/{id}
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<ProductDTO>> UpdateProduct(Guid id, UpdateProductDTO updateProductDTO)
    {
        var sellerIdClaim = User.FindFirst("userid")?.Value;

        if (string.IsNullOrEmpty(sellerIdClaim) || !Guid.TryParse(sellerIdClaim, out var sellerId))
            return Unauthorized(new { message = "Invalid token." });

        var product = await db.Products
            .Include(p => p.Seller)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound(new { message = "Product not found." });

        if (product.SellerId != sellerId)
            return Forbid();

        mapper.Map(updateProductDTO, product);
        await db.SaveChangesAsync();

        return Ok(mapper.Map<ProductDTO>(product));
    }

    // DELETE api/product/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var sellerIdClaim = User.FindFirst("userid")?.Value;

        if (string.IsNullOrEmpty(sellerIdClaim) || !Guid.TryParse(sellerIdClaim, out var sellerId))
            return Unauthorized(new { message = "Invalid token." });

        var product = await db.Products.FindAsync(id);

        if (product == null)
            return NotFound(new { message = "Product not found." });

        if (product.SellerId != sellerId)
            return Forbid();

        db.Products.Remove(product);
        await db.SaveChangesAsync();

        return NoContent();
    }
}
