using System.ComponentModel.DataAnnotations;

namespace uga_mpl_server.DTO.User;

public class UpdateUserDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Phone]
    public string? MobileNumber { get; set; }
}
