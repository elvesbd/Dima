using Dima.Core;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Api.Common.Api;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using Dima.Core.Requests.Orders;

namespace Dima.Api.Endpoints.Orders;

public abstract class GetAllProductsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("GetAllProducts")
            .WithSummary("Gets all products")
            .WithDescription("Gets all products")
            .WithOrder(2)
            .Produces<PagedResponse<List<Product>?>>();
    
    private static async Task<IResult> HandleAsync(
        IProductHandler handler,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetAllProductsRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        
        var result = await handler.GetAllAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest();
    }
}