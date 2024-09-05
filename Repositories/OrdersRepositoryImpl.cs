using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using simple_online_shop_be_dotnet.Data;
using simple_online_shop_be_dotnet.Models;

namespace simple_online_shop_be_dotnet.Repositories;

public class OrdersRepositoryImpl : OrdersRepository
{
    private readonly ApplicationDbContext _context;

    public OrdersRepositoryImpl(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Orders>> GetListOrdersAsync()
    {
        return await _context.Orders
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Orders> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task AddOrderAsync(Orders order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task UpdateOrderAsync(Orders order)
    {
        _context.Orders.Update(order);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountOrdersAsync()
    {
        return await _context.Orders.CountAsync();
    }
    
    public async Task DeleteOrderAsync(Orders orders)
    {
        _context.Orders.Remove(orders);
    }
    
    public async Task<bool> CustomerExistAsync(int customerId)
    {
        return await _context.Orders
            .AnyAsync(o => o.CustomerId == customerId);
    }
}