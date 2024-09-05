using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Dtos.Items;
using simple_online_shop_be_dotnet.Exceptions;
using simple_online_shop_be_dotnet.Models;
using simple_online_shop_be_dotnet.Repositories;

namespace simple_online_shop_be_dotnet.Services;

public class ItemsServiceImpl : ItemsService
{
    private readonly ItemsRepository _itemsRepository;
    public ItemsServiceImpl(ItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }
    
    public async Task<List<ItemsResponse>> GetListItems()
    {
        var items = await _itemsRepository.GetListItemsAsync();
        return items
            .Select(item => new ItemsResponse
            {
                ItemId = item.ItemId,
                ItemsName = item.ItemsName,
                ItemsCode = item.ItemsCode,
                Stock = item.Stock, 
                Price = item.Price,
                IsAvailable = item.IsAvailable,
                LastReStock = item.LastReStock
            })
            .ToList();
    }

    public async Task<ItemsDetailResponse> GetItemDetail(int itemId)
    {
        var item = await _itemsRepository.GetItemByIdAsync(itemId);
        if (item == null) 
            throw new NotFoundException("Item not found");
        
        return new ItemsDetailResponse
        {
            ItemId = item.ItemId,
            ItemsName = item.ItemsName,
            ItemsCode = item.ItemsCode,
            Stock = item.Stock, 
            Price = item.Price,
            IsAvailable = item.IsAvailable,
            LastReStock = item.LastReStock
        };
    }

    public async Task<ItemsResponse> AddItem(ItemsRequest itemRequest)
    {
        var item = new Items
        {
            ItemsName = itemRequest.ItemsName,
            ItemsCode = itemRequest.ItemsCode,
            Stock = itemRequest.Stock, 
            Price = itemRequest.Price,
            IsAvailable = true,
            LastReStock = DateTime.Now
        };
        await _itemsRepository.AddItemAsync(item);
        await _itemsRepository.SaveChangesAsync();
        
        return new ItemsResponse
        {
            ItemId = item.ItemId,
            ItemsName = item.ItemsName,
            ItemsCode = item.ItemsCode,
            Stock = item.Stock, 
            Price = item.Price,
            IsAvailable = item.IsAvailable,
            LastReStock = item.LastReStock
        };
    }

    public async Task<ItemsResponse> UpdateItem(int itemId, ItemsRequest itemRequest)
    {
        var item = await _itemsRepository.GetItemByIdAsync(itemId);
        if (item == null) 
            throw new NotFoundException("Item not found");
        
        item.ItemsName = itemRequest.ItemsName;
        item.ItemsCode = itemRequest.ItemsCode;
        item.Stock = itemRequest.Stock;
        item.Price = itemRequest.Price;
        item.LastReStock = DateTime.Now;
        
        await _itemsRepository.UpdateItemAsync(item);
        await _itemsRepository.SaveChangesAsync();
        
        return new ItemsResponse
        {
            ItemId = item.ItemId,
            ItemsName = item.ItemsName,
            ItemsCode = item.ItemsCode,
            Stock = item.Stock, 
            Price = item.Price,
            IsAvailable = item.IsAvailable,
            LastReStock = item.LastReStock
        };
    }

    public async Task<MessageResponse> DeleteItem(int itemId)
    {
        var item = await _itemsRepository.GetItemByIdAsync(itemId);
        if (item == null) 
            throw new NotFoundException("Item not found");
        
        item.IsAvailable = false;
        await _itemsRepository.UpdateItemAsync(item);
        await _itemsRepository.SaveChangesAsync();
        
        return new MessageResponse
        {
            Message = "Delete item successfully"
        };
    }
}