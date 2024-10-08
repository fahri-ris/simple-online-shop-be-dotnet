﻿using simple_online_shop_be_dotnet.Models;

namespace simple_online_shop_be_dotnet.Repositories;

public interface OrdersRepository
{
    Task<List<Orders>> GetListOrdersAsync();
    Task<Orders> GetOrderByIdAsync(int orderId);
    Task<Orders> GetOrderByCustomerAsync(int customerId);
    Task<Orders> GetOrderByItemAsync(int itemId);
    Task AddOrderAsync(Orders order);
    Task UpdateOrderAsync(Orders order);
    Task SaveChangesAsync();
    Task<int> CountOrdersAsync();
    Task<int> CountOrdersBySearchAsync(string search);
    Task DeleteOrderAsync(Orders orders);
    Task<bool> CustomerExistAsync(int customerId);
    Task<List<Orders>> GetListOrderIn(List<int> orderIds);
    Task<List<Orders>> GetPageOrders(int pageIndex, int pageSize);
    Task<List<Orders>> GetPageOrdersBySearch(int pageIndex, int pageSize, string search);
}