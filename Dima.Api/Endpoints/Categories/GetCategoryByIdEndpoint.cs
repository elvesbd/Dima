using System.Security.Claims;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Api.Common.Api;
using Dima.Core.Responses;
using Dima.Core.Requests.Categories;

namespace Dima.Api.Endpoints.Categories;

public abstract class GetCategoryByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id:long}", HandleAsync)
            .WithOrder(4)
            .WithName("GetCategoryById")
            .WithSummary("Get category by id")
            .WithDescription("Get category by id")
            .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(
        long id,
        ClaimsPrincipal user,
        ICategoryHandler handler)
    {
        var request = new GetCategoryByIdRequest
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