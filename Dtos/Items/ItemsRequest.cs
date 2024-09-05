namespace simple_online_shop_be_dotnet.Dtos.Items;

public class ItemsRequest
{
    public required string ItemsName { get; set; }
    public required string ItemsCode { get; set; }
    public int Stock { get; set; }
    public double Price { get; set; }
}