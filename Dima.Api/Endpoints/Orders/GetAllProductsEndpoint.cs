using Dima.Core;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Api.Common.Api;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using Dima.Core.Requests.Orders;

namespace Dima.Api.Endpoints.Orders;

public class GetAllProductsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("", HandleAsync)
            .WithName("GetAllProducts")
            .WithSummary("Gets all products")
            .WithDescription("Gets all products")
            .WithOrder(2)
            .Produces<PagedResponse<List<Product>?>>();
    
    private static async Task<IResult> HandleAsync(
        IProductHandler handler,
        [FromQuery] int pageNUmber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetAllProductsRequest
        {
            PageNumber = pageNUmber,
            PageSize = pageSize
        };
        
        var result = await handler.GetAllAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest();
    }
}