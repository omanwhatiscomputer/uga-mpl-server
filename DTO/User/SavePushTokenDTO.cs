using System.ComponentModel.DataAnnotations;

namespace uga_mpl_server.DTO.User;

public class SavePushTokenDTO
{
    [Required]
    public string ExpoPushToken { get; set; } = null!;
}
