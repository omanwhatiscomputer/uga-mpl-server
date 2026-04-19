using System.ComponentModel.DataAnnotations;

namespace uga_mpl_server.DTO.Transaction;

public class SellProductDTO
{
    [Required]
    public Guid BuyerId { get; set; }
}
