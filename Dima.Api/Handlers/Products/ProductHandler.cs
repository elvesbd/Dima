using Dima.Api.Data;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Core.Responses;
using Dima.Core.Requests.Orders;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers.Products;

public class ProductHandler(AppDbContext context) : IProductHandler
{
    public async Task<Response<Product?>> GetBySlugAsync(GetProductBySlugRequest request)
    {
        try
        {
            var product = await context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IsActive && x.Slug == request.Slug);
            
            return product is null
                ? new Response<Product?>(null, 404, "Product not found")
                : new Response<Product?>(product);
        }
        catch
        {
            return  new Response<Product?>(null, 500, "Not able to get product");
        }
    }

    public async Task<PagedResponse<List<Product>?>> GetAllAsync(GetAllProductsRequest request)
    {
        try
        {
            var query =  context.Products
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Title);

            var products = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
            
            var count  = await query.CountAsync();
            return new PagedResponse<List<Product>?>(products, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Product>?>(null, 500, "Not able to get products");
        }
    }
}