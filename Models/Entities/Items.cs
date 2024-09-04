using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace simple_online_shop_be_dotnet.Models.Entities;

[Table("items")]
public class Items
{
    [Key]
    public required int ItemId { get; set; }
    
    public required string ItemsName { get; set; }
    public required string ItemsCode { get; set; }
    public int Stock { get; set; }
    public double Price { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime LastReStock { get; set; }
}