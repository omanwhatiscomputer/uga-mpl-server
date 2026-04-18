namespace uga_mpl_server.DTO.Product;

public class ProductSummaryDTO
{
    public Guid Id { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
    public string Condition { get; set; } = null!;
    public string SellerName { get; set; } = null!;
    public bool IsAvailable { get; set; }
    public DateTime DateCreated { get; set; }
    public List<string> ProductImages { get; set; } = new List<string>();
}
