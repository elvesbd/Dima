using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Core.Responses;
using Dima.Api.Common.Api;
using System.Security.Claims;
using Dima.Core.Requests.Orders;

namespace Dima.Api.Endpoints.Orders;

public abstract class GetOrderByNumberEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{number}", HandleAsync)
        .WithName("GetByNumber")
        .WithSummary("Get order for a given number")
        .WithDescription("Get order for a given number")
        .WithOrder(5)
        .Produces<Response<Order?>>();

    private static async Task<IResult> HandleAsync(IOrderHandler handler, ClaimsPrincipal user, string number)
    {
        var request = new GetOrderByNumberRequest
        {
            UserId = user.Identity?.Name ?? string.Empty,
            Number = number
        };

        var result = await handler.GetByNumberAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}