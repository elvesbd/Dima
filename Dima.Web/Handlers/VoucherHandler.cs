using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Core.Responses;
using System.Net.Http.Json;
using Dima.Core.Requests.Orders;

namespace Dima.Web.Handlers;

public class VoucherHandler(IHttpClientFactory httpClientFactory) : IVoucherHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);

    public async Task<Response<Voucher?>> GetByNumberAsync(GetVoucherByNumberRequest request)
    {
        return await _client.GetFromJsonAsync<Response<Voucher?>>($"/v1/vouchers/{request.Number}")
               ?? new Response<Voucher?>(null, 400, "Não foi possível obter o voucher.");
    }
}