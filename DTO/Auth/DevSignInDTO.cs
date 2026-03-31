using System.ComponentModel.DataAnnotations;

namespace uga_mpl_server.DTO.Auth;

public class DevSignInDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;
}
