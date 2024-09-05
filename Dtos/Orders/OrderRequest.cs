namespace simple_online_shop_be_dotnet.Dtos.Orders;

public class OrderRequest
{
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int ItemsId { get; set; }
    public string? ItemsName { get; set; }
    public int Quantity { get; set; }
}