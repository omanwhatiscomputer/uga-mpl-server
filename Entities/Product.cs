using System;
using System.ComponentModel.DataAnnotations;
using uga_mpl_server.Enums;

namespace uga_mpl_server.Entities;

public class Product
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SellerId { get; set; }
    public User Seller { get; set; } = null!;
    public List<string> ProductImages { get; set; } = new List<string>();
    public string ProductName { get; set; } = null!;
    public string ProductDescription { get; set; } = null!;
    public decimal Price { get; set; }
    public string DateCreated { get; set; } = DateTime.UtcNow.ToString("o");
    public bool IsAvailable { get; set; } = true;
    public Category Category { get; set; }
    public ProductCondition Condition { get; set; }
    public List<Guid> WishlistedByUserIds { get; set; } = new List<Guid>();
    public List<Guid> SubscriberIds { get; set; } = new List<Guid>();
}