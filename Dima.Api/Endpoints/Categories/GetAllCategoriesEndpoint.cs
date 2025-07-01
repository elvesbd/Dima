using System.Security.Claims;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Responses;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Categories;

public abstract class GetAllCategoriesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithOrder(5)
            .WithName("GetAllCategories")
            .WithSummary("Get all category")
            .WithDescription("Get all category")
            .Produces<PagedResponse<List<Category?>>>();
    
    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ICategoryHandler handler,
        [FromQuery] int pageSize = Configuration.DefaultPageSize,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber)
    {
        var request = new GetAllCategoriesRequest
        {
            PageSize = pageSize,
            PageNumber = pageNumber,
            UserId = user.Identity?.Name ?? string.Empty
        };
        
        var result = await handler.GetAllAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}