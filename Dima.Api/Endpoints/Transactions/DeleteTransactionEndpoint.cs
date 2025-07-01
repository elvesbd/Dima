using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions;

public abstract class DeleteTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id:long}", HandleAsync)
            .WithOrder(3)
            .WithName("DeleteTransaction")
            .WithSummary("Delete transaction")
            .WithDescription("Delete transaction")
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(
        long id,
        ClaimsPrincipal user,
        ITransactionHandler handler)
    {
        var request = new DeleteTransactionRequest
        {
            Id = id,
            UserId = user.Identity?.Name ?? string.Empty
        };

        var result = await handler.DeleteAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}