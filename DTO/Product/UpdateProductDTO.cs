using System.ComponentModel.DataAnnotations;

namespace uga_mpl_server.DTO.Product;

public class UpdateProductDTO
{
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? Price { get; set; }

    public List<string>? ProductImages { get; set; }
}
