using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace simple_online_shop_be_dotnet.Models.Entities;

[Table("orders")]
public class Orders
{ 
    [Key]
    public required int OrderId { get; set; }
    
    public required string OrderCode { get; set; }
    public DateTime OrderDate { get; set; }
    public double TotalPrice { get; set; }
    
    public Customers? Customer { get; set; }
    public int CustomerId { get; set; }
    
    public Items? Items { get; set; }
    public int ItemsId { get; set; }
    
    public int Quantity { get; set; }
}