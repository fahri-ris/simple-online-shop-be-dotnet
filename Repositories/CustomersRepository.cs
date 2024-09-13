using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Dtos.Customers;
using simple_online_shop_be_dotnet.Models;

namespace simple_online_shop_be_dotnet.Repositories;

public interface CustomersRepository
{
    Task<List<Customers>> GetListCustomersAsync();
    Task<Customers> GetCustomerByIdAsync(int customerId);
    Task AddCustomerAsync(Customers customers);
    Task UpdateCustomerAsync(Customers customers);
    Task SaveChangesAsync();
    Task<int> CountCustomersAsync();
    Task<int> CountCustomersBySearchAsync(string search);
    Task DeleteCustomerAsync(Customers customers);
    Task<List<Customers>> GetPageCustomers(int pageIndex, int pageSize);
    Task<List<Customers>> GetPageCustomersBySearch(int pageIndex, int pageSize, string search);
}