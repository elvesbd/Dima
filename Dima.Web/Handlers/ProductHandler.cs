using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Core.Responses;
using System.Net.Http.Json;
using Dima.Core.Requests.Orders;

namespace Dima.Web.Handlers;

public class ProductHandler(IHttpClientFactory httpClientFactory) : IProductHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Response<Product?>> GetBySlugAsync(GetProductBySlugRequest request)
    {
        return await _client.GetFromJsonAsync<Response<Product?>>($"v1/products/{request.Slug}")
               ?? new Response<Product?>(null, 400, "Não foi possível obter o produto.");
    }

    public async Task<PagedResponse<List<Product>?>> GetAllAsync(GetAllProductsRequest request)
    {
        return await _client.GetFromJsonAsync<PagedResponse<List<Product>?>>("v1/products")
               ?? new PagedResponse<List<Product>?>(null, 400, "Não foi possível obter os produtos.");
    }
}