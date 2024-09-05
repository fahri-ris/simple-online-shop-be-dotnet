using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Dtos.Customers;
using simple_online_shop_be_dotnet.Exceptions;
using simple_online_shop_be_dotnet.Models;
using simple_online_shop_be_dotnet.Repositories;

namespace simple_online_shop_be_dotnet.Services;

public class CustomersServiceImpl : CustomersService
{
    private readonly CustomersRepository _customerRepository;

    public CustomersServiceImpl(CustomersRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<List<CustomersResponse>> GetListCustomer()
    {
        var customers = await _customerRepository.GetActiveCustomersAsync();
        return customers.Select(c => new CustomersResponse()
        {
            CustomerId = c.CustomerId,
            CustomerName = c.CustomerName,
            CustomerAddress = c.CustomerAddress,
            CustomerCode = c.CustomerCode,
            CustomerPhone = c.CustomerPhone,
            LastOrderDate = c.LastOrderDate
        }).ToList();
    }

    public async Task<CustomerDetailResponse> GetCustomerDetail(int customerId)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
        if (customer == null)
        {
            throw new NotFoundException("Customer not found");
        }

        return new CustomerDetailResponse()
        {
            CustomerId = customer.CustomerId,
            CustomerName = customer.CustomerName,
            CustomerAddress = customer.CustomerAddress,
            CustomerCode = customer.CustomerCode,
            CustomerPhone = customer.CustomerPhone,
            LastOrderDate = customer.LastOrderDate,
            IsActive = customer.IsActive,
            Pic = customer.Pic
        };
    }

    public async Task<CustomersResponse> AddCustomer(CustomerRequest customerRequest)
    {
        var customer = new Customers
        {
            CustomerName = customerRequest.CustomerName,
            CustomerAddress = customerRequest.CustomerAddress,
            CustomerCode = customerRequest.CustomerCode,
            CustomerPhone = customerRequest.CustomerPhone,
            LastOrderDate = DateTime.Now,
            IsActive = true,
            Pic = customerRequest.Pic
        };

        await _customerRepository.AddCustomerAsync(customer);
        await _customerRepository.SaveChangesAsync();
        
        return new CustomersResponse
        {
            CustomerId = customer.CustomerId,
            CustomerName = customer.CustomerName,
            CustomerAddress = customer.CustomerAddress,
            CustomerCode = customer.CustomerCode,
            CustomerPhone = customer.CustomerPhone,
            LastOrderDate = customer.LastOrderDate,
            IsActive = customer.IsActive,
            Pic = customer.Pic
        };
    }

    public async Task<CustomersResponse> UpdateCustomer(int customerId, CustomerRequest customerRequest)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
        if (customer == null) 
            throw new NotFoundException("Customer not found");

        customer.CustomerName = customerRequest.CustomerName;
        customer.CustomerAddress = customerRequest.CustomerAddress;
        customer.CustomerCode = customerRequest.CustomerCode;
        customer.CustomerPhone = customerRequest.CustomerPhone;
        customer.LastOrderDate = DateTime.Now;
        customer.Pic = customerRequest.Pic;

        await _customerRepository.UpdateCustomerAsync(customer);
        await _customerRepository.SaveChangesAsync();
        
        return new CustomersResponse()
        {
            CustomerId = customer.CustomerId,
            CustomerName = customer.CustomerName,
            CustomerAddress = customer.CustomerAddress,
            CustomerCode = customer.CustomerCode,
            CustomerPhone = customer.CustomerPhone,
            LastOrderDate = customer.LastOrderDate,
            IsActive = customer.IsActive,
            Pic = customer.Pic
        };
    }

    public async Task<MessageResponse> DeleteCustomer(int customerId)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
        if (customer == null)
            throw new NotFoundException("Customer not found");

        customer.IsActive = false;
        await _customerRepository.UpdateCustomerAsync(customer);
        await _customerRepository.SaveChangesAsync();
        
        var message = new MessageResponse()
        {
            Message = "Delete customer successfully"
        };
        return message;
    }
}