using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Dtos.Orders;
using simple_online_shop_be_dotnet.Exceptions;
using simple_online_shop_be_dotnet.Models;
using simple_online_shop_be_dotnet.Repositories;
using simple_online_shop_be_dotnet.Util;

namespace simple_online_shop_be_dotnet.Services;

public class OrderServiceImpl : OrderService
{
    private readonly OrdersRepository _ordersRepository;
    private readonly ItemsRepository _itemsRepository;
    private readonly CustomersRepository _customersRepository;
    private readonly CodeGenerator _codeGenerator;
    
    public OrderServiceImpl(OrdersRepository ordersRepository, ItemsRepository itemsRepository,
        CustomersRepository customersRepository, CodeGenerator codeGenerator)
    {
        _ordersRepository = ordersRepository;
        _itemsRepository = itemsRepository;
        _customersRepository = customersRepository;
        _codeGenerator = codeGenerator;
    }
    
    public async Task<List<OrdersResponse>> GetListOrders()
    {
        var orders = await _ordersRepository.GetListOrdersAsync();
        return orders.Select(
            order => new OrdersResponse()
            {
                OrderId = order.OrderId,
                OrderCode = order.OrderCode,
                CustomerName = order.Customers.CustomerName,
                ItemsName = order.Items.ItemsName,
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate
            }
            ).ToList();
    }

    public async Task<OrderDetailResponse> GetOrderDetail(int orderId)
    {
        var order = await _ordersRepository.GetOrderByIdAsync(orderId);
        if (order == null) 
            throw new NotFoundException("Order not found");
        
        return new OrderDetailResponse
        {
            OrderId = order.OrderId,
            OrderCode = order.OrderCode,
            CustomerId = order.CustomerId,
            CustomerName = order.Customers.CustomerName,
            ItemsId = order.ItemsId,
            ItemsName = order.Items.ItemsName,
            Quantity = order.Quantity,
            TotalPrice = order.TotalPrice,
            OrderDate = order.OrderDate
        };
    }

    public async Task<OrdersResponse> AddOrder(OrderRequest orderRequest)
    {
        // check customer status
        var customer = await _customersRepository.GetCustomerByIdAsync(orderRequest.CustomerId);
        if (customer.IsActive == false)
            throw new BadRequestException("Customer is inactive");
        
        // check stock of item
        var item = await _itemsRepository.GetItemByIdAsync(orderRequest.ItemsId);
        if (item.IsAvailable == false)
            throw new BadRequestException("Item is not available");
        
        if (item.Stock < orderRequest.Quantity)
            throw new BadRequestException("Stock is not enough");
        
        var order = new Orders
        {
            OrderCode = await _codeGenerator.GenerateOrderCode(),
            CustomerId = orderRequest.CustomerId,
            ItemsId = orderRequest.ItemsId,
            Quantity = orderRequest.Quantity,
            TotalPrice = item.Price * orderRequest.Quantity,
            OrderDate = DateTime.Now
        };

        await _ordersRepository.AddOrderAsync(order);
        
        // update quantity of item
        item.Stock -= orderRequest.Quantity;
        if (item.Stock == 0)
        {
            item.IsAvailable = false;
        }
        await _itemsRepository.UpdateItemAsync(item);
        
        // save order and item
        await _ordersRepository.SaveChangesAsync();
        await _itemsRepository.SaveChangesAsync();
        
        return new OrdersResponse
        {
            OrderId = order.OrderId,
            OrderCode = order.OrderCode,
            CustomerName = order.Customers.CustomerName,
            ItemsName = order.Items.ItemsName,
            Quantity = order.Quantity,
            TotalPrice = order.TotalPrice,
            OrderDate = order.OrderDate
        };
    }

    public async Task<OrdersResponse> UpdateOrder(int orderId, OrderRequest orderRequest)
    {
        var existingOrder = await _ordersRepository.GetOrderByIdAsync(orderId);
        if (existingOrder == null) 
            throw new NotFoundException("Order not found");
        
        // check stock of item
        var item = await _itemsRepository.GetItemByIdAsync(orderRequest.ItemsId);
        var currentQuantity = existingOrder.Quantity;
        var reqQuantity = orderRequest.Quantity;
        int newQuantity = 0;
        if (currentQuantity < reqQuantity)
        {
            newQuantity = reqQuantity - currentQuantity;
            if (item.Stock < newQuantity)
                throw new BadRequestException("Stock is not enough");
            
            item.Stock -= newQuantity;
        } else if (currentQuantity > reqQuantity)
        {
            newQuantity = currentQuantity - reqQuantity;
            item.Stock += newQuantity;
        }

        if (item.Stock == 0)
        {
            item.IsAvailable = false;
        }
        
        await _itemsRepository.UpdateItemAsync(item);
        
        existingOrder.Quantity = orderRequest.Quantity;
        existingOrder.TotalPrice = item.Price * orderRequest.Quantity;
        existingOrder.OrderDate = DateTime.Now;
        
        await _ordersRepository.UpdateOrderAsync(existingOrder);
        await _ordersRepository.SaveChangesAsync();

        return new OrdersResponse
        {
            OrderId = existingOrder.OrderId,
            OrderCode = existingOrder.OrderCode,
            CustomerName = existingOrder.Customers.CustomerName,
            ItemsName = existingOrder.Items.ItemsName,
            TotalPrice = existingOrder.TotalPrice,
            Quantity = existingOrder.Quantity,
        };
    }

    public async Task<MessageResponse> DeleteOrder(int orderId)
    {
        var order = await _ordersRepository.GetOrderByIdAsync(orderId);
        if (order == null) 
            throw new NotFoundException("Order not found");

        order.IsDeleted = true;
        await _ordersRepository.UpdateOrderAsync(order);
        await _ordersRepository.SaveChangesAsync();
        
        return new MessageResponse
        {
            Message = "Order deleted successfully"
        };
    }
}