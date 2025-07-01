using Dima.Api.Data;
using Dima.Core.Common;
using Dima.Core.Enums;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;
using Dima.Core.Requests.Transactions;

namespace Dima.Api.Handlers.Transactions;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        if (request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
            request.Amount *= -1;
        
        try
        {
            var transaction = new Transaction
            {
                Type = request.Type,
                Title = request.Title,
                Amount = request.Amount,
                UserId = request.UserId,
                CreatedAt = DateTime.Now,
                CategoryId = request.CategoryId,
                PaidOrReceived = request.PaidOrReceived
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(
                transaction,
                201,
                message: "Transaction created");
        }
        catch
        {
            return new Response<Transaction?>(
                null,
                500,
                message: "Not able to create transaction");
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        if (request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
            request.Amount *= -1;
        
        try
        {
            var transaction = await context
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
                return new Response<Transaction?>(
                    null,
                    404,
                    "Transaction not found");

            transaction.Type = request.Type;
            transaction.Title = request.Title;
            transaction.Amount = request.Amount;
            transaction.CategoryId = request.CategoryId;
            transaction.PaidOrReceived = request.PaidOrReceived;
            
            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, message: "Transaction updated");
        }
        catch
        {
            return new Response<Transaction?>(
                null,
                500,
                message: "Not able to update transaction");
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
                return new Response<Transaction?>(
                    null,
                    404,
                    "Transaction not found");
            
            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();

            return  new Response<Transaction?>(transaction, message: "Transaction deleted");
        }
        catch
        {
            return new Response<Transaction?>(
                null,
                500,
                "Not able to delete transaction");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return transaction is null
                ? new Response<Transaction?>(
                    null,
                    404,
                    "Transaction not found")
                : new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(
                null,
                500,
                "Not able to get transaction by id");
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        try
        {
            request.StartDate ??= DateTime.Now.GetFirstDay();
            request.EndDate ??= DateTime.Now.GetLastDay();
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(
                null,
                500,
                "Not able to get transactions by period");
        }
        
        try
        {
            var query = context
                .Transactions
                .AsNoTracking()
                .Where(x =>
                    x.PaidOrReceived >= request.StartDate &&
                    x.PaidOrReceived <= request.EndDate &&
                    x.UserId == request.UserId)
                .OrderBy(x => x.CreatedAt);

            var transactions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Transaction>?>(
                transactions,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(
                null,
                500,
                "Not able to get transactions by period");
        }
    }
}