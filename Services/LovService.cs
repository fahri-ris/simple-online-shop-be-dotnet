using simple_online_shop_be_dotnet.Dtos;

namespace simple_online_shop_be_dotnet.Services;

public interface LovService
{
    Task<List<LovResponse>> GetLovCustomer();
    Task<List<LovResponse>> GetLovItems();
}