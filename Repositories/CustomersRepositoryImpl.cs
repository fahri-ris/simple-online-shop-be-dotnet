using Microsoft.EntityFrameworkCore;
using simple_online_shop_be_dotnet.Data;
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

    public async Task<List<Customers>> GetActiveCustomersAsync()
    {
        return await _context.Customers
            .Where(c => c.IsActive)
            .ToListAsync();
    }

    public async Task<Customers> GetCustomerByIdAsync(int customerId)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customerId && c.IsActive);
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
}