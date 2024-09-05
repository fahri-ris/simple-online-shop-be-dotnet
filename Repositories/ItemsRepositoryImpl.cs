using Microsoft.EntityFrameworkCore;
using simple_online_shop_be_dotnet.Data;
using simple_online_shop_be_dotnet.Models;

namespace simple_online_shop_be_dotnet.Repositories;

public class ItemsRepositoryImpl : ItemsRepository
{
    private readonly ApplicationDbContext _context;
    public ItemsRepositoryImpl(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Items>> GetListItemsAsync()
    {
        return await _context.Items
            .Where(item => item.IsAvailable)
            .ToListAsync();
    }

    public async Task<Items> GetItemByIdAsync(int itemId)
    {
        return await _context.Items
            .FirstOrDefaultAsync(i => i.ItemId == itemId && i.IsAvailable);
    }

    public async Task AddItemAsync(Items item)
    {
        await _context.Items.AddAsync(item);
    }

    public async Task UpdateItemAsync(Items item)
    {
        _context.Items.Update(item);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}