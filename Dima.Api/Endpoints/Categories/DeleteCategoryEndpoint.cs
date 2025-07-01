using System.Security.Claims;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Api.Common.Api;
using Dima.Core.Responses;
using Dima.Core.Requests.Categories;

namespace Dima.Api.Endpoints.Categories;

public abstract class DeleteCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id:long}", HandleAsync)
            .WithOrder(3)
            .WithName("DeleteCategory")
            .WithSummary("Delete category")
            .WithDescription("Delete categoru")
            .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(
        long id,
        ClaimsPrincipal user,
        ICategoryHandler handler)
    {
        var request = new DeleteCategoryRequest
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