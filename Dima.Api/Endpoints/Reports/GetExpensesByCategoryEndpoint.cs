using Dima.Core.Handlers;
using Dima.Api.Common.Api;
using System.Security.Claims;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Reports;

public abstract class GetExpensesByCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/expenses", HandleAsync)
            .WithOrder(1)
            .WithName("GetExpensesByCategory")
            .WithSummary("Get Expenses By Category")
            .WithDescription("Get Expenses By Category")
            .Produces<Response<List<ExpensesByCategory>?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IReportsHandler handler)
    {
        var request = new GetExpensesByCategoryRequest
        {
            UserId = user.Identity?.Name ?? string.Empty
        };
        
        var result = await handler.GetExpensesByCategoryReportAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}