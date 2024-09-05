using Microsoft.AspNetCore.Mvc;
using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Dtos.Customers;
using simple_online_shop_be_dotnet.Dtos.Items;
using simple_online_shop_be_dotnet.Exceptions;
using simple_online_shop_be_dotnet.Services;

namespace simple_online_shop_be_dotnet.Controllers;

[ApiController]
[Route("api/items")]
public class ItemsController : ControllerBase
{
    private readonly ItemsService _itemsService;

    public ItemsController(ItemsService itemsService)
    {
        _itemsService = itemsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ItemsResponse>>> GetListItems()
    {
        var items = await _itemsService.GetListItems();
        return Ok(items);
    }
    
    [HttpGet("{itemId}")]
    public async Task<ActionResult<ItemsResponse>> GetDetailItem(int itemId)
    {
        try
        {
            var item = await _itemsService.GetItemDetail(itemId);
            return Ok(item);
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
    public async Task<ActionResult<CustomersResponse>> AddItem([FromBody] ItemsRequest itemsRequest)
    {
        var item = await _itemsService.AddItem(itemsRequest);
        return Ok(item);
    }
    
    [HttpPut("{itemId}")]
    public async Task<ActionResult<CustomersResponse>> UpdateItem([FromBody] ItemsRequest itemsRequest, int itemId)
    {
        try
        {
            var updatedItem = await _itemsService.UpdateItem(itemId, itemsRequest);
            return Ok(updatedItem);
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
    
    [HttpDelete("{itemId}")]
    public async Task<ActionResult<MessageResponse>> DeleteItem(int itemId)
    {
        try
        {
            var result = await _itemsService.DeleteItem(itemId);
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