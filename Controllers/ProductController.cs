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

        return CreatedAtAction(nameof(CreateProduct), mapper.Map<ProductDTO>(product));
    }
}
