﻿using Microsoft.EntityFrameworkCore;
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
            .Where(i => i.IsDeleted == false)
            .OrderByDescending(i => i.LastReStock)
            .ToListAsync();
    }
    
    public async Task<List<Items>> GetListItemsOrderByNameAsc()
    {
        return await _context.Items
            .Where(i => i.IsDeleted == false)
            .OrderBy(i => i.ItemsName)
            .ToListAsync();
    }

    public async Task<Items> GetItemByIdAsync(int itemId)
    {
        return await _context.Items
            .FirstOrDefaultAsync(i => i.ItemId == itemId);
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
    
    public async Task<int> ItemsCountAsync()
    {
        return await _context.Items
            .Where(i => i.IsDeleted == false)
            .CountAsync();
    }
    
    public async Task<int> ItemsCountBySearchAsync(string search)
    {
        return await _context.Items
            .Where(i => i.IsDeleted == false && i.ItemsName.ToLower().Contains(search.ToLower()))
            .CountAsync();
    }
    
    public async Task DeleteItemAsync(Items item)
    {
        _context.Items.Remove(item);
    }
    
    public async Task<List<Items>> GetPageItems(int pageIndex, int pageSize)
    {
        var items = await _context.Items
            .Where(i => i.IsDeleted == false)
            .OrderByDescending(i => i.LastReStock)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return items;
    }
    
    public async Task<List<Items>> GetPageItemsBySearch(int pageIndex, int pageSize, string search)
    {
        var items = await _context.Items
            .Where(i => i.IsDeleted == false && i.ItemsName.ToLower().Contains(search.ToLower()))
            .OrderByDescending(i => i.LastReStock)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return items;
    }
}