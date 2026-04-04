using uga_mpl_server.DTO.Product;

namespace uga_mpl_server.DTO.User;

public class UserDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string MobileNumber { get; set; } = null!;
    public string DateJoined { get; set; } = null!;
    public List<ProductSummaryDTO> Wishlist { get; set; } = new List<ProductSummaryDTO>();
    public List<ProductSummaryDTO> Subscriptions { get; set; } = new List<ProductSummaryDTO>();
}
