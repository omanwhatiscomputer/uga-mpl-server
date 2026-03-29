using System.ComponentModel.DataAnnotations;

namespace uga_mpl_server.DTO.User;

public class CreateUserDTO
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [Phone]
    public string MobileNumber { get; set; } = null!;
}
