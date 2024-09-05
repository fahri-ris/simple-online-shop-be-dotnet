using simple_online_shop_be_dotnet.Models;

namespace simple_online_shop_be_dotnet.Repositories;

public interface ItemsRepository
{
    Task<List<Items>> GetListItemsAsync();
    Task<Items> GetItemByIdAsync(int itemId);
    Task AddItemAsync(Items item);
    Task UpdateItemAsync(Items item);
    Task SaveChangesAsync();
}