using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Dtos.Customers;
using simple_online_shop_be_dotnet.Models;

namespace simple_online_shop_be_dotnet.Services;

public interface CustomersService
{
    Task<List<CustomersResponse>> GetListCustomer();
    Task<CustomerDetailResponse> GetCustomerDetail(int customerId);
    Task<CustomersResponse> AddCustomer(CustomerRequest customerRequest);
    Task<CustomersResponse> UpdateCustomer(int customerId, CustomerRequest customerRequest);
    Task<MessageResponse> DeleteCustomer(int customerId);
    Task<PaginationResponse<Customers>> GetPageCustomers(int pageIndex, int pageSize);
}