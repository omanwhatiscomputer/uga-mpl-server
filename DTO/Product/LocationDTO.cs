using System.ComponentModel.DataAnnotations;

namespace uga_mpl_server.DTO.Product;

public class LocationDTO
{
    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Range(-180, 180)]
    public double Longitude { get; set; }
}
