using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Core.Responses;
using Dima.Api.Common.Api;
using Dima.Core.Requests.Orders;

namespace Dima.Api.Endpoints.Orders;

public abstract class GetVoucherByNumberEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{number}", HandleAsync)
            .WithName("GetVoucherByNumber")
            .WithDescription("Gets a Voucher by number")
            .WithSummary("Gets a Voucher by number")
            .WithOrder(1)
            .Produces<Response<Voucher?>>();

    private static async Task<IResult> HandleAsync(IVoucherHandler handler, string number)
    {
        var request = new GetVoucherByNumberRequest
        {
            Number = number
        };
        
        var result = await handler.GetByNumberAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest();
    }
}