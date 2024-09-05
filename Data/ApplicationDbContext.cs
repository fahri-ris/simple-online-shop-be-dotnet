using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using simple_online_shop_be_dotnet.Models;

namespace simple_online_shop_be_dotnet.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Customers> Customers { get; set; }
    public DbSet<Items> Items { get; set; }
    public DbSet<Orders> Orders { get; set; }
}