using System;
using System.ComponentModel.DataAnnotations;

namespace uga_mpl_server.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string MobileNumber { get; set; } = null!;
    public string DateJoined { get; set; } = DateTime.UtcNow.ToString("o");
    public List<Guid> WishlistedProductIds { get; set; } = new List<Guid>();
    public List<Guid> SubscribedProductIds { get; set; } = new List<Guid>();
}