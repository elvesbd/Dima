using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Api.Common.Api;
using Dima.Core.Responses;
using System.Security.Claims;
using Dima.Core.Requests.Orders;

namespace Dima.Api.Endpoints.Orders;

public abstract class GetProductBySlugEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{slug}", HandleAsync)
            .WithName("GetProductBySlug")
            .WithSummary("Get order for a given slug")
            .WithDescription("Get order for a given slug")
            .WithOrder(1)
            .Produces<Response<Product?>>();

    private static async Task<IResult> HandleAsync(IProductHandler handler, string slug, ClaimsPrincipal user)
    {
        var request = new GetProductBySlugRequest
        {
            Slug = slug
        };
        
        var result = await handler.GetBySlugAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest();
    }
}