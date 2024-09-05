namespace simple_online_shop_be_dotnet.Dtos.Customers;

public class CustomerRequest
{
    public required string CustomerName { get; set; }
    public string? CustomerAddress { get; set; }
    public string? CustomerPhone { get; set; }
    public bool IsActive { get; set; }
    public string? Pic { get; set; }
}