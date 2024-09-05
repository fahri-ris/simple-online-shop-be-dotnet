using simple_online_shop_be_dotnet.Repositories;

namespace simple_online_shop_be_dotnet.Util;

public class CodeGenerator
{
    private readonly OrdersRepository _ordersRepository;
    private readonly ItemsRepository _itemsRepository;
    private readonly CustomersRepository _customersRepository;

    public CodeGenerator(OrdersRepository ordersRepository, ItemsRepository itemsRepository, CustomersRepository customersRepository)
    {
        _ordersRepository = ordersRepository;
        _itemsRepository = itemsRepository;
        _customersRepository = customersRepository;
    }
    
    public async Task<string> GenerateCustomerCode()
    {
        var prefix = "CUS-";
        var defaultNumber = 00001;
        var lastCode = await _customersRepository.CountCustomersAsync();

        string customerCode;
        if (lastCode == 0)
        {
            customerCode = prefix + defaultNumber;
        }
        else
        {
            lastCode++;
            customerCode = prefix + lastCode;
        }

        return customerCode;
    }
    
    public async Task<string> GenerateItemCode()
    {
        var prefix = "I-";
        var defaultNumber = 00001;
        var lastCode = await _itemsRepository.ItemsCountAsync();

        string itemCode;
        if (lastCode == 0)
        {
            itemCode = prefix + defaultNumber;
        }
        else
        {
            lastCode++;
            itemCode = prefix + lastCode;
        }

        return itemCode;
    }
    
    public async Task<string> GenerateOrderCode()
    {
        var prefix = "ORD-";
        var defaultNumber = 00001;
        var lastCode = await _ordersRepository.CountOrdersAsync();

        string orderCode;
        if (lastCode == 0)
        {
            orderCode = prefix + defaultNumber;
        }
        else
        {
            lastCode++;
            orderCode = prefix + lastCode;
        }

        return orderCode;
    }
}