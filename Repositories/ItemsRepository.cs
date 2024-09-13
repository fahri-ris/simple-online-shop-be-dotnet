using simple_online_shop_be_dotnet.Models;

namespace simple_online_shop_be_dotnet.Repositories;

public interface ItemsRepository
{
    Task<List<Items>> GetListItemsAsync();
    Task<List<Items>> GetListItemsOrderByNameAsc();
    Task<Items> GetItemByIdAsync(int itemId);
    Task AddItemAsync(Items item);
    Task UpdateItemAsync(Items item);
    Task SaveChangesAsync();
    Task<int> ItemsCountAsync();
    Task<int> ItemsCountBySearchAsync(string search);
    Task DeleteItemAsync(Items items);
    Task<List<Items>> GetPageItems(int pageIndex, int pageSize);
    Task<List<Items>> GetPageItemsBySearch(int pageIndex, int pageSize, string search);
}