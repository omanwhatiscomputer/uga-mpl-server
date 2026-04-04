using System.ComponentModel.DataAnnotations;

namespace uga_mpl_server.DTO.Product;

public class CreateProductDTO
{
    [Required]
    public string ProductName { get; set; } = null!;

    [Required]
    public string ProductDescription { get; set; } = null!;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    public List<string> ProductImages { get; set; } = new List<string>();

    [Required]
    public string Category { get; set; } = null!;

    [Required]
    public string Condition { get; set; } = null!;
}
