using uga_mpl_server.DTO.User;

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
    public bool IsAvailable { get; set; }
    public string Category { get; set; } = null!;
    public string Condition { get; set; } = null!;
    public List<UserSummaryDTO> Subscribers { get; set; } = new List<UserSummaryDTO>();
    public List<UserSummaryDTO> WishlistedBy { get; set; } = new List<UserSummaryDTO>();
}
