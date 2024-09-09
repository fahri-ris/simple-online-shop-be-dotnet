﻿using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Dtos.Items;
using simple_online_shop_be_dotnet.Exceptions;
using simple_online_shop_be_dotnet.Models;
using simple_online_shop_be_dotnet.Repositories;
using simple_online_shop_be_dotnet.Util;

namespace simple_online_shop_be_dotnet.Services;

public class ItemsServiceImpl : ItemsService
{
    private readonly ItemsRepository _itemsRepository;
    private readonly CodeGenerator _codeGenerator;
    private readonly OrdersRepository _ordersRepository;
    
    public ItemsServiceImpl(ItemsRepository itemsRepository, CodeGenerator codeGenerator,
        OrdersRepository ordersRepository)
    {
        _itemsRepository = itemsRepository;
        _codeGenerator = codeGenerator;
        _ordersRepository = ordersRepository;
    }
    
    public async Task<List<ItemsResponse>> GetListItems()
    {
        var items = await _itemsRepository.GetListItemsAsync();
        return items
            .Select(item => new ItemsResponse
            {
                ItemId = item.ItemId,
                ItemsName = item.ItemsName,
                ItemsCode = item.ItemsCode,
                Stock = item.Stock, 
                Price = item.Price,
                IsAvailable = item.IsAvailable,
                LastReStock = item.LastReStock
            })
            .ToList();
    }

    public async Task<ItemsDetailResponse> GetItemDetail(int itemId)
    {
        var item = await _itemsRepository.GetItemByIdAsync(itemId);
        if (item == null) 
            throw new NotFoundException("Item not found");
        
        return new ItemsDetailResponse
        {
            ItemId = item.ItemId,
            ItemsName = item.ItemsName,
            ItemsCode = item.ItemsCode,
            Stock = item.Stock, 
            Price = item.Price,
            IsAvailable = item.IsAvailable,
            LastReStock = item.LastReStock
        };
    }

    public async Task<ItemsResponse> AddItem(ItemsRequest itemRequest)
    {
        var item = new Items
        {
            ItemsName = itemRequest.ItemsName,
            ItemsCode = await _codeGenerator.GenerateItemCode(),
            Stock = itemRequest.Stock, 
            Price = itemRequest.Price,
            IsAvailable = true,
            LastReStock = DateTime.Now
        };
        await _itemsRepository.AddItemAsync(item);
        await _itemsRepository.SaveChangesAsync();
        
        return new ItemsResponse
        {
            ItemId = item.ItemId,
            ItemsName = item.ItemsName,
            ItemsCode = item.ItemsCode,
            Stock = item.Stock, 
            Price = item.Price,
            IsAvailable = item.IsAvailable,
            LastReStock = item.LastReStock
        };
    }

    public async Task<ItemsResponse> UpdateItem(int itemId, ItemsRequest itemRequest)
    {
        var item = await _itemsRepository.GetItemByIdAsync(itemId);
        if (item == null) 
            throw new NotFoundException("Item not found");
        
        item.ItemsName = itemRequest.ItemsName;
        item.Stock = itemRequest.Stock;
        item.Price = itemRequest.Price;
        item.LastReStock = DateTime.Now;
        
        await _itemsRepository.UpdateItemAsync(item);
        await _itemsRepository.SaveChangesAsync();
        
        return new ItemsResponse
        {
            ItemId = item.ItemId,
            ItemsName = item.ItemsName,
            ItemsCode = item.ItemsCode,
            Stock = item.Stock, 
            Price = item.Price,
            IsAvailable = item.IsAvailable,
            LastReStock = item.LastReStock
        };
    }

    public async Task<MessageResponse> DeleteItem(int itemId)
    {
        var item = await _itemsRepository.GetItemByIdAsync(itemId);
        if (item == null) 
            throw new NotFoundException("Item not found");

        item.IsDeleted = true;
        await _itemsRepository.UpdateItemAsync(item);
        
        var order = await _ordersRepository.GetOrderByItemAsync(itemId);
        if (order == null)
            throw new NotFoundException("Order not found");
        order.IsDeleted = true;
        await _ordersRepository.UpdateOrderAsync(order);
        
        // save all changes
        await _itemsRepository.SaveChangesAsync();
        await _ordersRepository.SaveChangesAsync();
        
        return new MessageResponse
        {
            Message = "Delete item successfully"
        };
    }
}