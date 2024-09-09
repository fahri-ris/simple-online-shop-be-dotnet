using Microsoft.AspNetCore.Mvc;
using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Dtos.Customers;
using simple_online_shop_be_dotnet.Dtos.Items;
using simple_online_shop_be_dotnet.Dtos.Orders;
using simple_online_shop_be_dotnet.Exceptions;
using simple_online_shop_be_dotnet.Services;

namespace simple_online_shop_be_dotnet.Controllers;

[ApiController]
[Route("api/order")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;
    
    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<OrdersResponse>>> GetListOrder()
    {
        var orders = await _orderService.GetListOrders();
        return Ok(orders);
    }
    
    [HttpGet("{orderId}")]
    public async Task<ActionResult<OrderDetailResponse>> GetDetailOrder(int orderId)
    {
        try
        {
            var order = await _orderService.GetOrderDetail(orderId);
            return Ok(order);
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
    public async Task<ActionResult<OrdersResponse>> AddOrder([FromBody] OrderRequest orderRequest)
    {
        try
        {
            var order = await _orderService.AddOrder(orderRequest);
            return Ok(order);
        }
        
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
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
    
    [HttpPut("{orderId}")]
    public async Task<ActionResult<CustomersResponse>> UpdateOrder([FromBody] OrderRequest orderRequest, int orderId)
    {
        try
        {
            var updatedOrder = await _orderService.UpdateOrder(orderId, orderRequest);
            return Ok(updatedOrder);
        }
        
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
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
    
    [HttpDelete("{orderId}")]
    public async Task<ActionResult<MessageResponse>> DeleteOrder(int orderId)
    {
        try
        {
            var result = await _orderService.DeleteOrder(orderId);
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
    
    [HttpPost("download-pdf")]
    public async Task<ActionResult> DownloadPdf([FromBody] OrderDownloadPdfRequest orderDownloadPdfRequests)
    {
        try
        {
            var pdfDocument = await _orderService.DownloadPdf(orderDownloadPdfRequests);
            Response.ContentType = "application/pdf";
            
            return File(pdfDocument, "application/pdf", "Order.pdf");
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