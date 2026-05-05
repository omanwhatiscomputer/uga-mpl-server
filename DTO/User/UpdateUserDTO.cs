using System.ComponentModel.DataAnnotations;

namespace uga_mpl_server.DTO.User;

public class UpdateUserDTO
{
    [Phone]
    public string MobileNumber { get; set; }
}
