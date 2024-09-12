using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using simple_online_shop_be_dotnet.Data;
using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Dtos.Customers;
using simple_online_shop_be_dotnet.Exceptions;
using simple_online_shop_be_dotnet.Models;
using simple_online_shop_be_dotnet.Repositories;
using simple_online_shop_be_dotnet.Services;

namespace simple_online_shop_be_dotnet.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly CustomersService _customersService;

    public CustomersController(CustomersService customersService)
    {
        _customersService = customersService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomersResponse>>> GetCustomerList()
    {
        var customers = await _customersService.GetListCustomer();
        return Ok(customers);
    }
    
    [HttpGet("pagination")]
    public async Task<ActionResult<PaginationResponse<Customers>>> GetCustomerPagination(int pageIndex = 1, int pageSize = 10)
    {
        return await _customersService.GetPageCustomers(pageIndex, pageSize);
    }
    
    [HttpGet("{customerId}")]
    public async Task<ActionResult<CustomerDetailResponse>> GetDetailCustomer(int customerId)
    {
        try
        {
            var customer = await _customersService.GetCustomerDetail(customerId);
            return Ok(customer);
        }

        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        
    }

    [HttpPost]
    public async Task<ActionResult<CustomersResponse>> AddCustomer([FromBody] CustomerRequest customerRequest)
    {
        var customer = await _customersService.AddCustomer(customerRequest);
        return Ok(customer);
    }
    
    [HttpPut("{customerId}")]
    public async Task<ActionResult<CustomersResponse>> UpdateCustomer([FromBody] CustomerRequest customerRequest, int customerId)
    {
        try
        {
            var updatedCustomer = await _customersService.UpdateCustomer(customerId, customerRequest);
            return Ok(updatedCustomer);
        }
        
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    
    [HttpDelete("{customerId}")]
    public async Task<ActionResult<MessageResponse>> DeleteCustomer(int customerId)
    {
        try
        {
            var result = await _customersService.DeleteCustomer(customerId);
            return Ok(result);
        }
        
        
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}