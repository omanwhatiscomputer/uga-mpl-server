using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using uga_mpl_server.Data;
using uga_mpl_server.DTO.Transaction;

namespace uga_mpl_server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionController(ApplicationDBContext db, IMapper mapper) : ControllerBase
{
    // GET api/transaction/sales
    [HttpGet("sales")]
    public async Task<ActionResult<List<TransactionDTO>>> GetSales()
    {
        var userIdClaim = User.FindFirst("userid")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Invalid token." });

        var transactions = await db.Transactions
            .Where(t => t.SellerId == userId)
            .Include(t => t.Product)
            .Include(t => t.Seller)
            .Include(t => t.Buyer)
            .ToListAsync();

        return Ok(mapper.Map<List<TransactionDTO>>(transactions));
    }

    // GET api/transaction/purchases
    [HttpGet("purchases")]
    public async Task<ActionResult<List<TransactionDTO>>> GetPurchases()
    {
        var userIdClaim = User.FindFirst("userid")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Invalid token." });

        var transactions = await db.Transactions
            .Where(t => t.BuyerId == userId)
            .Include(t => t.Product)
            .Include(t => t.Seller)
            .Include(t => t.Buyer)
            .ToListAsync();

        return Ok(mapper.Map<List<TransactionDTO>>(transactions));
    }
}
