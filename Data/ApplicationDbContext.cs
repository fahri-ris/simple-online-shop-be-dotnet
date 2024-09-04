using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using simple_online_shop_be_dotnet.Models.Entities;

namespace simple_online_shop_be_dotnet.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySQL(
                _configuration.GetConnectionString("Default")!
            );
        }
    }
    
    public DbSet<Customers> Customers { get; set; }
    public DbSet<Items> Items { get; set; }
    public DbSet<Orders> Orders { get; set; }
}