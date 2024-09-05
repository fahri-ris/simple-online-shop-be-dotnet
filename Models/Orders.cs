using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace simple_online_shop_be_dotnet.Models;

[Table("orders")]
public class Orders
{ 
    [Key]
    [Column("order_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId { get; set; }
    
    [Column("order_code")]
    public required string OrderCode { get; set; }
    
    [Column("order_date")]
    public DateTime OrderDate { get; set; }
    
    [Column("total_price")]
    public double TotalPrice { get; set; }
    
    [Column("customer_id")]
    public int CustomerId { get; set; }
    
    [Column("items_id")]
    public int ItemsId { get; set; }
    
    [Column("quantity")]
    public int Quantity { get; set; }
}