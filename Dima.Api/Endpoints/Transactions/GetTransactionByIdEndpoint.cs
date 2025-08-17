using System.Security.Claims;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Api.Common.Api;
using Dima.Core.Responses;
using Dima.Core.Requests.Transactions;

namespace Dima.Api.Endpoints.Transactions;

public abstract class GetTransactionByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id:long}", HandleAsync)
            .WithOrder(4)
            .WithName("GetTransactionById")
            .WithSummary("Get transaction by id")
            .WithDescription("Get transaction by id")
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(
        long id,
        ClaimsPrincipal user,
        ITransactionHandler handler)
    {
        var request = new GetTransactionByIdRequest()
        {   
            Id = id,
            UserId = user.Identity?.Name ?? string.Empty
        };
        
        var result = await handler.GetByIdAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.NotFound(result);
    }
}