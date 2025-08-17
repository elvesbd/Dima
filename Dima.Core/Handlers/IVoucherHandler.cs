using Dima.Core.Models;
using Dima.Core.Responses;
using Dima.Core.Requests.Orders;

namespace Dima.Core.Handlers;

public interface IVoucherHandler
{
    Task<Response<Voucher?>> GetByNumberAsync(GetVoucherByNumberRequest request);
}