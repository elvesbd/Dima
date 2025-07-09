using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Reports;

public abstract class GetIncomesByCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/incomes", HandleAsync)
            .WithOrder(3)
            .WithName("GetIncomesByCategory")
            .WithSummary("GetI Incomes By Category")
            .WithDescription("Get Incomes By Category")
            .Produces<Response<List<IncomesByCategory>?>>();
    
    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IReportsHandler handler)
    {
        var request = new GetIncomesByCategoryRequest
        {
            UserId = user.Identity?.Name ?? string.Empty
        };
        
        var result = await handler.GetIncomesByCategoryReportAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}