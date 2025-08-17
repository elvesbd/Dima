using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions;

public abstract class GetTransactionsByPeriod : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithOrder(4)
            .WithName("GetTransactionByPeriod")
            .WithSummary("Get transaction by period")
            .WithDescription("Get transaction by period")
            .Produces<PagedResponse<List<Transaction>>?>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ITransactionHandler handler,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] int pageSize = Configuration.DefaultPageSize,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber)
    {
        var request = new GetTransactionsByPeriodRequest
        {   
            EndDate = endDate,
            StartDate = startDate,
            PageSize = pageSize,
            PageNumber = pageNumber,
            UserId = user.Identity?.Name ?? string.Empty
        };
        
        var result = await handler.GetByPeriodAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.NotFound(result);
    }
}