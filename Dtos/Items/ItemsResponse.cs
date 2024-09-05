namespace simple_online_shop_be_dotnet.Dtos.Items;

public class ItemsResponse
{
    public int ItemId { get; set; }
    public string? ItemsName { get; set; }
    public string? ItemsCode { get; set; }
    public int Stock { get; set; }
    public double Price { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime LastReStock { get; set; }
}