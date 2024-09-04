using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace simple_online_shop_be_dotnet.Models.Entities;

[Table("customers")]
public class Customers
{
    [Key]
    public required int CustomerId { get; set; }

    public required string CustomerName { get; set; }
    public string? CustomerAddress { get; set; }
    public required string CustomerCode { get; set; }
    public string? CustomerPhone { get; set; }
    public bool IsActive { get; set; }
    public DateTime LastOrderDate { get; set; }
    public string? Pic { get; set; }
}