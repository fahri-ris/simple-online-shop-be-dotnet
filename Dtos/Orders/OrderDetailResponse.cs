namespace simple_online_shop_be_dotnet.Dtos.Orders;

public class OrderDetailResponse
{
    public int OrderId { get; set; }
    public string? OrderCode { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int ItemsId { get; set; }
    public string? ItemsName { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
}