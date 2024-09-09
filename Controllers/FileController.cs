using Microsoft.AspNetCore.Mvc;
using Minio;
using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Services;

namespace simple_online_shop_be_dotnet.Controllers;

[ApiController]
[Route("api/file")]
public class FileController : ControllerBase
{
    private readonly MinioService _minioService;

    public FileController(MinioService minioService)
    {
        _minioService = minioService;
    }

    [HttpPost("upload")]
    public async Task<ActionResult<FileResponse>> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var response = await _minioService.UploadImage(file);

        if (response == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading file.");
        }

        return Ok(response);
    }

    [HttpGet("get/{fileName}")]
    public async Task<ActionResult<FileResponse>> GetImage(string fileName)
    {
        var response = await _minioService.GetImage(fileName);
        if (response == null)
        {
            return NotFound("File not found.");
        }

        return Ok(response);
    }
    
    
}