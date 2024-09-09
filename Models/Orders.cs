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
    
    [ForeignKey("CustomerId")]
    public Customers Customers { get; set; } = null!;
    
    [Column("items_id")]
    public int ItemsId { get; set; }
    
    [ForeignKey("ItemsId")]
    public Items Items { get; set; } = null!;
    
    [Column("quantity")]
    public int Quantity { get; set; }
    
    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
}