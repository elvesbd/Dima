using System.Security.Claims;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Api.Common.Api;
using Dima.Core.Responses;
using Dima.Core.Requests.Categories;

namespace Dima.Api.Endpoints.Categories;

public abstract class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithOrder(1)
            .WithName("CreateCategory")
            .WithSummary("Create new category")
            .WithDescription("Create new category")
            .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ICategoryHandler handler,
        CreateCategoryRequest request)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        
        var result = await handler.CreateAsync(request);
        return result.IsSuccess
            ? TypedResults.Created($"/{result.Data?.Id}",result)
            : TypedResults.BadRequest(result);
    } 
}