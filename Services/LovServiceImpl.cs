using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Repositories;

namespace simple_online_shop_be_dotnet.Services;

public class LovServiceImpl : LovService
{
    private readonly CustomersRepository _customersRepository;
    private readonly ItemsRepository _itemsRepository;
    
    public LovServiceImpl(CustomersRepository customersRepository, ItemsRepository itemsRepository)
    {
        _customersRepository = customersRepository;
        _itemsRepository = itemsRepository;
    }

    public async Task<List<LovResponse>> GetLovCustomer()
    {
        var customers = await _customersRepository.GetListCustomersAsync();
        return customers
            .Select(c => new LovResponse
            {
                Id = c.CustomerId,
                Data = c.CustomerName
            })
            .ToList();
    }

    public async Task<List<LovResponse>> GetLovItems()
    {
        var items = await _itemsRepository.GetListItemsOrderByNameAsc();
        return items
            .Select(c => new LovResponse
            {
                Id = c.ItemId,
                Data = c.ItemsName
            })
            .ToList();
    }
}