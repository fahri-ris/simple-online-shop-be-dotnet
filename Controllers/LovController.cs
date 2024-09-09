using Microsoft.AspNetCore.Mvc;
using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Services;

namespace simple_online_shop_be_dotnet.Controllers;

[ApiController]
[Route("api/lov")]
public class LovController
{
    private readonly LovService _lovService;

    public LovController(LovService lovService)
    {
        _lovService = lovService;
    }

    [HttpGet("customers")]
    public async Task<ActionResult<List<LovResponse>>> GetListCustomer()
    {
        return await _lovService.GetLovCustomer();
    }
    
    [HttpGet("items")]
    public async Task<ActionResult<List<LovResponse>>> GetListItem()
    {
        return await _lovService.GetLovItems();
    }
}