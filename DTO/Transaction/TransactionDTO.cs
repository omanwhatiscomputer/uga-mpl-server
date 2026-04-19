using uga_mpl_server.DTO.User;

namespace uga_mpl_server.DTO.Transaction;

public class TransactionDTO
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public string Date { get; set; } = null!;
    public UserSummaryDTO Seller { get; set; } = null!;
    public UserSummaryDTO Buyer { get; set; } = null!;
}
