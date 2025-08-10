using Dima.Api.Data;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Core.Responses;
using Dima.Core.Requests.Orders;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers.Vouchers;

public class VoucherHandler(AppDbContext context) : IVoucherHandler
{
    public async Task<Response<Voucher?>> GetByNumberAsync(GetVoucherByNumberRequest request)
    {
        try
        {
            var voucher = await context.Vouchers
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Number == request.Number && v.IsActive == true);

            return voucher is null
                ? new Response<Voucher?>(null, 404, "Voucher not found")
                : new Response<Voucher?>(voucher);
        }
        catch
        {
            return new Response<Voucher?>(null, 400, "Voucher not found");
        }
    }
}