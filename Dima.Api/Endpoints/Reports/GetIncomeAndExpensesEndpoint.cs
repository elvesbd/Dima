using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Reports;

public abstract class GetIncomeAndExpensesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/incomes-expenses", HandleAsync)
            .WithOrder(4)
            .WithName("GetIncomeAndExpenses")
            .WithSummary("Get Income And Expenses")
            .WithDescription("Get Income And Expenses")
            .Produces<Response<List<IncomesAndExpenses>?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IReportsHandler handler)
    {
        var request = new GetIncomesAndExpensesRequest
        {
            UserId = user.Identity?.Name ?? string.Empty
        };
        
        var result = await handler.GetIncomesAndExpensesReportAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}