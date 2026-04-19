using System.ComponentModel.DataAnnotations;

namespace uga_mpl_server.Entities;

public class Transaction
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public Guid SellerId { get; set; }
    public User Seller { get; set; } = null!;
    public Guid BuyerId { get; set; }
    public User Buyer { get; set; } = null!;
    public decimal Price { get; set; }
    public string Date { get; set; } = DateTime.UtcNow.ToString("o");
}
