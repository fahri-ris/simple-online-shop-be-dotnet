using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace simple_online_shop_be_dotnet.Models;

[Table("customers")]
public class Customers
{
    [Key]
    [Column("customer_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerId { get; set; }

    [Column("customer_name")]
    public required string CustomerName { get; set; }
    
    [Column("customer_address")]
    public string? CustomerAddress { get; set; }
    
    [Column("customer_code")]
    public required string CustomerCode { get; set; }
    
    [Column("customer_phone")]
    public string? CustomerPhone { get; set; }
    
    [Column("is_active")]
    public bool IsActive { get; set; }
    
    [Column("last_order_date")]
    public DateTime LastOrderDate { get; set; }
    
    [Column("pic")]
    public string? Pic { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
}