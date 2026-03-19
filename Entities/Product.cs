using System;
using System.ComponentModel.DataAnnotations;

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
}