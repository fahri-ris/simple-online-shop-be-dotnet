using Minio;
using Minio.DataModel.Args;
using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Repositories;

namespace simple_online_shop_be_dotnet.Services;

public class MinioServiceImpl : MinioService
{
    private IMinioClient _minioClient;
    
    public MinioServiceImpl(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }
    
    public async Task<FileResponse> UploadImage(IFormFile file)
    {
        var bucketName= "dotnet-test-gap";
        
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        await using (var stream = file.OpenReadStream())
        {
            await _minioClient.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithContentType(file.ContentType)
            );
        }
        
        var args = new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithExpiry(24 * 60 * 60); // 24 hours

        var fileLink = await _minioClient.PresignedGetObjectAsync(args);

        return new FileResponse()
        {
            Filename = fileName,
            Url = fileLink
        };
    }

    public async Task<FileResponse> GetImage(string fileName)
    {
        var bucketName= "dotnet-test-gap";
        var statObject = await _minioClient.StatObjectAsync(
            new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
        );

        var args = new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithExpiry(10 * 60); // 10 minutes

        var fileLink = await _minioClient.PresignedGetObjectAsync(args);

        return new FileResponse()
        {
            Filename = fileName,
            Url = fileLink
        };
    }

}