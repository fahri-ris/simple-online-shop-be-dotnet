using Microsoft.EntityFrameworkCore;
using simple_online_shop_be_dotnet.Data;
using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Dtos.Customers;
using simple_online_shop_be_dotnet.Models;

namespace simple_online_shop_be_dotnet.Repositories;

public class CustomersRepositoryImpl : CustomersRepository
{
    private readonly ApplicationDbContext _context;

    public CustomersRepositoryImpl(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customers>> GetListCustomersAsync()
    {
        return await _context.Customers
            .Where(c => c.IsDeleted == false)
            .OrderBy(c => c.CustomerName)
            .ToListAsync();
    }

    public async Task<Customers> GetCustomerByIdAsync(int customerId)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }

    public async Task AddCustomerAsync(Customers customer)
    {
        await _context.Customers.AddAsync(customer);
    }

    public async Task UpdateCustomerAsync(Customers customer)
    {
        _context.Customers.Update(customer);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task<int> CountCustomersAsync()
    {
        return await _context.Customers
            .CountAsync();
    }
    
    public async Task DeleteCustomerAsync(Customers customers)
    {
        _context.Customers.Remove(customers);
    }

    public async Task<List<Customers>> GetPageCustomers(int pageIndex, int pageSize)
    {
        var customers = await _context.Customers
            .Where(c => c.IsDeleted == false)
            .OrderBy(c => c.CustomerName)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return customers;
    }
}