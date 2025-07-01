using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions;

public abstract class CreateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithOrder(1)
            .WithName("CreateTransaction")
            .WithSummary("Create new transaction")
            .WithDescription("Create new transaction")
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(
        ITransactionHandler handler,
        ClaimsPrincipal user,
        CreateTransactionRequest request)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        
        var result = await handler.CreateAsync(request);
        return result.IsSuccess
            ? TypedResults.Created($"/{result.Data?.Id}",result)
            : TypedResults.BadRequest(result);
    }
}