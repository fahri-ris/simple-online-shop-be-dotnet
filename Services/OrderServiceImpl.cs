using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Dtos.Orders;
using simple_online_shop_be_dotnet.Exceptions;
using simple_online_shop_be_dotnet.Models;
using simple_online_shop_be_dotnet.Repositories;
using simple_online_shop_be_dotnet.Util;

namespace simple_online_shop_be_dotnet.Services;

public class OrderServiceImpl : OrderService
{
    private readonly OrdersRepository _ordersRepository;
    private readonly ItemsRepository _itemsRepository;
    private readonly CustomersRepository _customersRepository;
    private readonly CodeGenerator _codeGenerator;
    
    public OrderServiceImpl(OrdersRepository ordersRepository, ItemsRepository itemsRepository,
        CustomersRepository customersRepository, CodeGenerator codeGenerator)
    {
        _ordersRepository = ordersRepository;
        _itemsRepository = itemsRepository;
        _customersRepository = customersRepository;
        _codeGenerator = codeGenerator;
    }
    
    public async Task<List<OrdersResponse>> GetListOrders()
    {
        var orders = await _ordersRepository.GetListOrdersAsync();
        return orders.Select(
            order => new OrdersResponse()
            {
                OrderId = order.OrderId,
                OrderCode = order.OrderCode,
                CustomerName = order.Customers.CustomerName,
                ItemsName = order.Items.ItemsName,
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate
            }
            ).ToList();
    }

    public async Task<OrderDetailResponse> GetOrderDetail(int orderId)
    {
        var order = await _ordersRepository.GetOrderByIdAsync(orderId);
        if (order == null) 
            throw new NotFoundException("Order not found");
        
        return new OrderDetailResponse
        {
            OrderId = order.OrderId,
            OrderCode = order.OrderCode,
            CustomerId = order.CustomerId,
            CustomerName = order.Customers.CustomerName,
            ItemsId = order.ItemsId,
            ItemsName = order.Items.ItemsName,
            Quantity = order.Quantity,
            TotalPrice = order.TotalPrice,
            OrderDate = order.OrderDate
        };
    }

    public async Task<OrdersResponse> AddOrder(OrderRequest orderRequest)
    {
        // check customer status
        var customer = await _customersRepository.GetCustomerByIdAsync(orderRequest.CustomerId);
        if (customer.IsActive == false)
            throw new BadRequestException("Customer is inactive");
        
        // check stock of item
        var item = await _itemsRepository.GetItemByIdAsync(orderRequest.ItemsId);
        if (item.IsAvailable == false)
            throw new BadRequestException("Item is not available");
        
        if (item.Stock < orderRequest.Quantity)
            throw new BadRequestException("Stock is not enough");
        
        var order = new Orders
        {
            OrderCode = await _codeGenerator.GenerateOrderCode(),
            CustomerId = orderRequest.CustomerId,
            ItemsId = orderRequest.ItemsId,
            Quantity = orderRequest.Quantity,
            TotalPrice = item.Price * orderRequest.Quantity,
            OrderDate = DateTime.Now
        };

        await _ordersRepository.AddOrderAsync(order);
        
        // update quantity of item
        item.Stock -= orderRequest.Quantity;
        if (item.Stock == 0)
        {
            item.IsAvailable = false;
        }
        await _itemsRepository.UpdateItemAsync(item);
        
        // save order and item
        await _ordersRepository.SaveChangesAsync();
        await _itemsRepository.SaveChangesAsync();
        
        return new OrdersResponse
        {
            OrderId = order.OrderId,
            OrderCode = order.OrderCode,
            CustomerName = order.Customers.CustomerName,
            ItemsName = order.Items.ItemsName,
            Quantity = order.Quantity,
            TotalPrice = order.TotalPrice,
            OrderDate = order.OrderDate
        };
    }

    public async Task<OrdersResponse> UpdateOrder(int orderId, OrderRequest orderRequest)
    {
        var existingOrder = await _ordersRepository.GetOrderByIdAsync(orderId);
        if (existingOrder == null) 
            throw new NotFoundException("Order not found");
        
        // check stock of item
        var item = await _itemsRepository.GetItemByIdAsync(orderRequest.ItemsId);
        var currentQuantity = existingOrder.Quantity;
        var reqQuantity = orderRequest.Quantity;
        int newQuantity = 0;
        if (currentQuantity < reqQuantity)
        {
            newQuantity = reqQuantity - currentQuantity;
            if (item.Stock < newQuantity)
                throw new BadRequestException("Stock is not enough");
            
            item.Stock -= newQuantity;
        } else if (currentQuantity > reqQuantity)
        {
            newQuantity = currentQuantity - reqQuantity;
            item.Stock += newQuantity;
        }

        if (item.Stock == 0)
        {
            item.IsAvailable = false;
        }
        
        await _itemsRepository.UpdateItemAsync(item);
        
        existingOrder.Quantity = orderRequest.Quantity;
        existingOrder.TotalPrice = item.Price * orderRequest.Quantity;
        existingOrder.OrderDate = DateTime.Now;
        
        await _ordersRepository.UpdateOrderAsync(existingOrder);
        await _ordersRepository.SaveChangesAsync();

        return new OrdersResponse
        {
            OrderId = existingOrder.OrderId,
            OrderCode = existingOrder.OrderCode,
            CustomerName = existingOrder.Customers.CustomerName,
            ItemsName = existingOrder.Items.ItemsName,
            TotalPrice = existingOrder.TotalPrice,
            Quantity = existingOrder.Quantity,
        };
    }

    public async Task<MessageResponse> DeleteOrder(int orderId)
    {
        var order = await _ordersRepository.GetOrderByIdAsync(orderId);
        if (order == null) 
            throw new NotFoundException("Order not found");

        order.IsDeleted = true;
        await _ordersRepository.UpdateOrderAsync(order);
        await _ordersRepository.SaveChangesAsync();
        
        return new MessageResponse
        {
            Message = "Order deleted successfully"
        };
    }

    public async Task<byte[]> DownloadPdf(OrderDownloadPdfRequest orderDownloadPdfRequests)
    {
        // order list
        var orders = await _ordersRepository.GetListOrderIn(orderDownloadPdfRequests.OrderId);
        if (orders == null)
            throw new NotFoundException("Data Not Found");
        
        var document = new Document();
        
        // create document template
        BuildDocument(document, orders);
        
        // render
        var rendererDocument = new PdfDocumentRenderer();
        rendererDocument.Document = document;
        
        // render document to pdf
        rendererDocument.RenderDocument();
        PdfDocument pdfDocumentResponse = rendererDocument.PdfDocument;
        
        // download
        MemoryStream stream = new MemoryStream();
        pdfDocumentResponse.Save(stream);
        
        byte[] bytes = stream.ToArray();
        stream.Close();

        return bytes;
    }

    public void BuildDocument(Document document, List<Orders> orders)
    {
        Section section = document.AddSection();
        Paragraph paragraph = section.AddParagraph();
        paragraph.AddText("Order Report");
        paragraph.Format.Font.Bold = true;
        paragraph.Format.Font.Size = 20;
        paragraph.Format.Alignment = ParagraphAlignment.Center;
        
        TextFrame addressFrame;
        addressFrame = section.AddTextFrame();
        // addressFrame.LineFormat.Width = 0.5; //Only for visual purposes
        addressFrame.Height = "15.0cm";//any number
        addressFrame.Width = "17.0cm";//sum of col widths
        addressFrame.Left = ShapePosition.Center;
        addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;//irrelevant
        addressFrame.Top = "4.0cm";//irrelevant
        addressFrame.RelativeVertical = RelativeVertical.Page;//irrelevant
        
        // create item table
        var table = addressFrame.AddTable();
        table.Style = "Table";
        table.Borders.Color = Colors.Black;
        table.Borders.Width = 0.5;
        table.Borders.Left.Width = 0.5;
        table.Borders.Right.Width = 0.5;
        table.Format.Alignment = ParagraphAlignment.Center;
    
        // header
        Column column = table.AddColumn("1cm");
        column.Format.Alignment = ParagraphAlignment.Center;
        column = table.AddColumn("2cm");
        column.Format.Alignment = ParagraphAlignment.Center;
        column = table.AddColumn("4cm");
        column.Format.Alignment = ParagraphAlignment.Center;
        column = table.AddColumn("4cm");
        column.Format.Alignment = ParagraphAlignment.Center;
        column = table.AddColumn("2cm");
        column.Format.Alignment = ParagraphAlignment.Center;
        column = table.AddColumn("2cm");
        column.Format.Alignment = ParagraphAlignment.Center;
        column = table.AddColumn("2cm");
        column.Format.Alignment = ParagraphAlignment.Center;
        
        Row row = table.AddRow();
        row.HeadingFormat = true;
        row.Format.Font.Bold = true;
        row.Cells[0].AddParagraph("No");
        row.Cells[1].AddParagraph("Order Code");
        row.Cells[2].AddParagraph("Customer Name");
        row.Cells[3].AddParagraph("Items Name");
        row.Cells[4].AddParagraph("Quantity");
        row.Cells[5].AddParagraph("Total Price");
        row.Cells[6].AddParagraph("Order Date");

        int no = 1;
        foreach (var order in orders)
        {
            row = table.AddRow();
            row.Cells[0].AddParagraph(no.ToString());
            row.Cells[1].AddParagraph(order.OrderCode);
            row.Cells[2].AddParagraph(order.Customers.CustomerName);
            row.Cells[3].AddParagraph(order.Items.ItemsName);
            row.Cells[4].AddParagraph(order.Quantity.ToString());
            row.Cells[5].AddParagraph(order.TotalPrice.ToString());
            row.Cells[6].AddParagraph(order.OrderDate.ToString());

            no++;
        }
    }
}