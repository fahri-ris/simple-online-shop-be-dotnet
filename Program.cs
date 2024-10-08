using Microsoft.EntityFrameworkCore;
using Minio;
using MySqlConnector;
using simple_online_shop_be_dotnet.Data;
using simple_online_shop_be_dotnet.Repositories;
using simple_online_shop_be_dotnet.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// minio
var endpoint = "play.min.io";
var accessKey = "minioadmin";
var secretKey = "minioadmin";

builder.Services.AddSingleton(new MinioClient()
    .WithEndpoint(endpoint)
    .WithCredentials(accessKey, secretKey)
    .WithSSL()
    .Build());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// mySQL connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("Default")!));

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()
    .AddClasses(classes => classes.InNamespaces("simple_online_shop_be_dotnet.Services", 
        "simple_online_shop_be_dotnet.Repositories"))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Util
builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()
    .AddClasses(classes => classes.InNamespaces("simple_online_shop_be_dotnet.Util"))
    .AsSelf()
    .WithScopedLifetime());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();
app.MapControllers();
app.Run();
