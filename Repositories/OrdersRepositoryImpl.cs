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
            .Where(o => o.IsDeleted == false)
            .Include(o => o.Customers)
            .Include(o => o.Items)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Orders> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.Customers)
            .Include(o => o.Items)
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

    public async Task<Orders> GetOrderByCustomerAsync(int customerId)
    {
        return await _context.Orders
            .Include(o => o.Customers)
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.CustomerId == customerId);
    }
    
    public async Task<Orders> GetOrderByItemAsync(int itemId)
    {
        return await _context.Orders
            .Include(o => o.Customers)
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.ItemsId == itemId);
    }

    public async Task<List<Orders>> GetListOrderIn(List<int> orderIds)
    {
        return await _context.Orders
            .Where(o => orderIds.Contains(o.OrderId))
            .Include(o => o.Customers)
            .Include(o => o.Items)
            .ToListAsync();
    }
    
    public async Task<List<Orders>> GetPageOrders(int pageIndex, int pageSize)
    {
        var orders = await _context.Orders
            .Where(o => o.IsDeleted == false)
            .Include(o => o.Customers)
            .Include(o => o.Items)
            .OrderByDescending(o => o.OrderDate)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return orders;
    }
}