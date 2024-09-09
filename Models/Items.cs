using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace simple_online_shop_be_dotnet.Models;

[Table("items")]
public class Items
{
    [Key]
    [Column("items_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ItemId { get; set; }
    
    [Column("items_name")]
    public required string ItemsName { get; set; }
    
    [Column("items_code")]
    public required string ItemsCode { get; set; }
    
    [Column("stock")]
    public int Stock { get; set; }
    
    [Column("price")]
    public double Price { get; set; }
    
    [Column("is_available")]
    public bool IsAvailable { get; set; }
    
    [Column("last_re_stock")]
    public DateTime LastReStock { get; set; }
    
    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
}