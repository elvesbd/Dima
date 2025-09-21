using Dima.Api.Data;
using Dima.Core.Enums;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Core.Responses;
using Dima.Core.Requests.Orders;
using Dima.Core.Requests.Stripe;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers.Orders;

public class OrderHandler(AppDbContext context, IStripeHandler stripeHandler) : IOrderHandler
{
    public async Task<Response<Order?>> PayAsync(PayOrderRequest request)
    {
        Order? order;
        try
        {
            order = await context.Orders
                .Include(x => x.Product)
                .Include(x => x.Voucher)
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            
            if (order is null)
                return new Response<Order?>(null, 404,  "Order not found");
        }
        catch
        {
           return new Response<Order?>(null, 500,  "Not able to get order");
        }
        
        switch (order.Status)
        {
            case EOrderStatus.Canceled:
                return new Response<Order?>(null, 400,  "Order canceled");
            case EOrderStatus.WaitingPayment:
                break;
            case EOrderStatus.Paid:
                return new Response<Order?>(null, 400,  "Order paid");
            case EOrderStatus.Refunded:
                return new Response<Order?>(null, 400,  "Order refunded cannot be payed");
            default:
                return new Response<Order?>(null, 400,  "Cannot pay order");
        }
        
        // stripe
        try
        {
            var getTransactionsRequest = new GetTransactionsByOrderNumberRequest
            {
                Number = order.Number
            };
            
            var result = await stripeHandler.GetTransactionsByOrderNumberAsync(getTransactionsRequest);
            if (result.IsSuccess == false || result.Data is null)
                return new Response<Order?>(null, 500, "Não foi possível localizar o pagamento");

            if (result.Data.Any(x => x.Refunded))
                return new Response<Order?>(null, 400, "Este pedido já teve o pagamento reembolsado");
            
            if (!result.Data.Any(x => x.Paid))
                return new Response<Order?>(null, 400, "Este pedido não foi pago");

            request.ExternalReference = result.Data[0].Id;
        }
        catch
        {
            return new Response<Order?>(null, 500, "Não foi possível dar baixa no pedido");
        }
        
        order.Status = EOrderStatus.Paid;
        order.ExternalReference = request.ExternalReference;
        order.UpdatedAt = DateTime.Now;

        try
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            return new Response<Order?>(null, 500,  "Not able to payed order");
        }
        
        return new Response<Order?>(order, 200, $"order {order.Number} paid");
    }

    public async Task<Response<Order?>> RefundAsync(RefundOrderRequest request)
    {
        Order? order;

        try
        {
            order = await context.Orders
                .Include(x => x.Product)
                .Include(x => x.Voucher)
                .FirstOrDefaultAsync(o => o.Id ==request.Id && o.UserId == request.UserId);
            
            if (order is null) return new Response<Order?>(null, 404, "Order not found");
        }
        catch
        {
            return new Response<Order?>(null, 500, "Not able to refund order");
        }

        switch (order.Status)
        {
            case EOrderStatus.Canceled:
                return new Response<Order?>(null, 400,  "Order canceled cannot be refunded");
            case EOrderStatus.WaitingPayment:
                return new Response<Order?>(null, 400, "Order waiting payment cannot be refunded");
            case EOrderStatus.Paid:
                break;
            case EOrderStatus.Refunded:
                return new Response<Order?>(null, 400,  "Order refunded");
            default:
                return new Response<Order?>(null, 400,  "Cannot refunded order");
        }
        
        order.Status = EOrderStatus.Refunded;
        order.UpdatedAt = DateTime.Now;

        try
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            return new Response<Order?>(null, 400,  "Cannot refunded order");
        }
        
        return new Response<Order?>(order, 200, $"Order {order.Number} refunded with success");
    }

    public async Task<Response<Order?>> CreateAsync(CreateOrderRequest request)
    {
        Product? product;
        try
        {
            product = await context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.ProductId && p.IsActive);
            
            if (product is null)
                return new Response<Order?>(null, 404, "Product not found");
            
            context.Attach(product);
        }
        catch
        {
            return new Response<Order?>(null, 500,  "Not able to get product");
        }

        Voucher? voucher = null;
        try
        {
            if (request.VoucherId is not null)
            {
                voucher = await context.Vouchers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(v => v.Id == request.VoucherId && v.IsActive);
                
                if (voucher is null)
                    return new Response<Order?>(null, 404, "Voucher not found");
                
                voucher.IsActive = false;
                context.Vouchers.Update(voucher);
            }
        }
        catch
        {
            return new Response<Order?>(null, 500,  "Not able to get voucher");
        }

        var order = new Order
        {
            UserId = request.UserId,
            Product = product,
            ProductId = request.ProductId,
            Voucher = voucher,
            VoucherId = request.VoucherId,
        };

        try
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            return new Response<Order?>(null, 500,  "Not able to add order");
        }
        
        return new Response<Order?>(order, 201, $"Order {order.Number} created");
    }

    public async Task<Response<Order?>> CancelAsync(CancelOrderRequest request)
    {
        Order? order;
        try
        {
            order = await context.Orders
                .Include(x => x.Product)
                .Include(x => x.Voucher)
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            
            if (order is null)
                return new Response<Order?>(null, 404,  "Order not found");
        }
        catch
        {
            return new Response<Order?>(null, 500,  "not able to get order");
        }

        switch (order.Status)
        {
            case EOrderStatus.Canceled:
                return new Response<Order?>(null, 400,  "Order canceled");
            case EOrderStatus.WaitingPayment:
                break;
            case EOrderStatus.Paid:
                return new Response<Order?>(null, 400,  "Order paid cannot be canceled");
            case EOrderStatus.Refunded:
                return new Response<Order?>(null, 400,  "Order refunded cannot be canceled");
            default:
                return new Response<Order?>(null, 400,  "Order has been canceled");
        }
        
        order.Status = EOrderStatus.Canceled;
        order.UpdatedAt = DateTime.Now;

        try
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            return new Response<Order?>(null, 500,  "Order could not be canceled");
        }
        
        return new Response<Order?>(order, 200, $"Pedido {order.Number} canceled with success");
    }

    public async Task<Response<Order?>> GetByNumberAsync(GetOrderByNumberRequest request)
    {
        try
        {
            var order = await context.Orders
                .AsNoTracking()
                .Include(o => o.Product)
                .Include(o => o.Voucher)
                .FirstOrDefaultAsync(o => o.Number == request.Number && o.UserId == request.UserId);
            
            return order is null 
                ? new Response<Order?>(null, 404, $"Order {request.Number} not found")
                : new Response<Order?>(order);
        }
        catch
        {
            return new Response<Order?>(null, 500, $"Cannot be get order {request.Number}");
        }
    }

    public async Task<PagedResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request)
    {
        try
        {
            var query = context.Orders
                .AsNoTracking()
                .Include(o => o.Product)
                .Include(o => o.Voucher)
                .Where(o => o.UserId == request.UserId)
                .OrderByDescending(o => o.CreatedAt);

            var orders = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Order>?>(orders, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Order>?>(null, 500, "Cannot be get orders");
        }
    }
}