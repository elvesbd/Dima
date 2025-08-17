using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Api.Common.Api;
using Dima.Core.Responses;
using System.Security.Claims;
using Dima.Core.Requests.Orders;

namespace Dima.Api.Endpoints.Orders;

public abstract class RefundOrderEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/{id:long}/refund", HandleAsync)
            .WithName("RefundOrder")
            .WithSummary("Refunds an order")
            .WithDescription("Refunds an order")
            .WithOrder(2)
            .Produces<Response<Order?>>();
    
    private static async Task<IResult> HandleAsync(IOrderHandler handler, long id, ClaimsPrincipal user)
    {
        var request = new RefundOrderRequest
        {
            Id = id,
            UserId = user.Identity?.Name ?? string.Empty
        };
        
        var result = await handler.RefundAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}