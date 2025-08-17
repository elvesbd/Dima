using Dima.Core.Models;
using Dima.Core.Responses;
using Dima.Core.Requests.Orders;

namespace Dima.Core.Handlers;

public interface IProductHandler
{
    Task<Response<Product?>> GetBySlugAsync(GetProductBySlugRequest request);
    Task<PagedResponse<List<Product>?>> GetAllAsync(GetAllProductsRequest request);
}