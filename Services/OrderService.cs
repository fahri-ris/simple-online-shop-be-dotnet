using simple_online_shop_be_dotnet.Dtos;
using simple_online_shop_be_dotnet.Dtos.Orders;

namespace simple_online_shop_be_dotnet.Services;

public interface OrderService
{
    Task<List<OrdersResponse>> GetListOrders();
    Task<OrderDetailResponse> GetOrderDetail(int orderId);
    Task<OrdersResponse> AddOrder(OrderRequest orderRequest);
    Task<OrdersResponse> UpdateOrder(int orderId, OrderRequest orderRequest);
    Task<MessageResponse> DeleteOrder(int orderId);
}