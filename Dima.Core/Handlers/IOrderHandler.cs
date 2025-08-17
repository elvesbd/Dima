using Dima.Core.Models;
using Dima.Core.Responses;
using Dima.Core.Requests.Orders;

namespace Dima.Core.Handlers;

public interface IOrderHandler
{
    Task<Response<Order?>> PayAsync(PayOrderRequest request);
    Task<Response<Order?>> RefundAsync(RefundOrderRequest request);
    Task<Response<Order?>> CreateAsync(CreateOrderRequest request);
    Task<Response<Order?>> CancelAsync(CancelOrderRequest request);
    Task<Response<Order?>> GetByNumberAsync(GetOrderByNumberRequest request);
    Task<PagedResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request);
}