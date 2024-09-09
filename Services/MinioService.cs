using simple_online_shop_be_dotnet.Dtos;

namespace simple_online_shop_be_dotnet.Services;

public interface MinioService
{
    Task<FileResponse> UploadImage(IFormFile file);
    Task<FileResponse> GetImage(string fileName);
}