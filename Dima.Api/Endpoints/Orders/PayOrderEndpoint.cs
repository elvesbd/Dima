using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Api.Common.Api;
using Dima.Core.Responses;
using System.Security.Claims;
using Dima.Core.Requests.Orders;

namespace Dima.Api.Endpoints.Orders;

public abstract class PayOrderEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/{number}/pay", HandleAsync)
            .WithName("PayOrder")
            .WithSummary("Pay Order")
            .WithDescription("Pay Order")
            .WithOrder(1)
            .Produces<Response<Order?>>();

    private static async Task<IResult> HandleAsync(IOrderHandler handler, string number, PayOrderRequest request,
        ClaimsPrincipal user)
    {
        request.Number = number;
        request.UserId = user.Identity?.Name ?? string.Empty;
        
        var result = await handler.PayAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest();
    }
}