namespace uga_mpl_server.DTO.Product;

public class ProductDTO
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public string SellerName { get; set; } = null!;
    public List<string> ProductImages { get; set; } = new List<string>();
    public string ProductName { get; set; } = null!;
    public string ProductDescription { get; set; } = null!;
    public decimal Price { get; set; }
    public string DateCreated { get; set; } = null!;
}
