using System.Security.Claims;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Api.Common.Api;
using Dima.Core.Responses;
using Dima.Core.Requests.Categories;

namespace Dima.Api.Endpoints.Categories;

public abstract class UpdateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id:long}", HandleAsync)
            .WithOrder(2)
            .WithName("UpdateCategory")
            .WithSummary("Update category")
            .WithDescription("Update category")
            .Produces<Response<Category?>>();
    
    private static async Task<IResult> HandleAsync(
        long id,
        ClaimsPrincipal user,
        ICategoryHandler handler,
        UpdateCategoryRequest request)
    {
        request.Id = id;
        request.UserId = user.Identity?.Name ?? string.Empty;
        
        var result = await handler.UpdateAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}