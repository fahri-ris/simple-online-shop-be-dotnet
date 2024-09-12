using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Dtos.Items;
using simple_online_shop_be_dotnet.Models;

namespace simple_online_shop_be_dotnet.Services;

public interface ItemsService
{
    Task<List<ItemsResponse>> GetListItems();
    Task<ItemsDetailResponse> GetItemDetail(int itemId);
    Task<ItemsResponse> AddItem(ItemsRequest itemRequest);
    Task<ItemsResponse> UpdateItem(int itemId, ItemsRequest itemRequest);
    Task<MessageResponse> DeleteItem(int itemId);
    Task<PaginationResponse<Items>> GetPageItems(int pageIndex, int pageSize);
}