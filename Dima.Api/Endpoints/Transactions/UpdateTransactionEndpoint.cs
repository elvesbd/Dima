using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions;

public abstract class UpdateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id:long}", HandleAsync)
            .WithOrder(2)
            .WithName("UpdateTransaction")
            .WithSummary("Update transaction")
            .WithDescription("Update transaction")
            .Produces<Response<Transaction?>>();
    
    private static async Task<IResult> HandleAsync(
        long id,
        ClaimsPrincipal user,
        ITransactionHandler handler,
        UpdateTransactionRequest request)
    {
        request.Id = id;
        request.UserId = user.Identity?.Name ?? string.Empty;
        
        var result = await handler.UpdateAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}