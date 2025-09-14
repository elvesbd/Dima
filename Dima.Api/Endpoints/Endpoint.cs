using Dima.Api.Common.Api;
using Dima.Api.Endpoints.Categories;
using Dima.Api.Endpoints.Identity;
using Dima.Api.Endpoints.Orders;
using Dima.Api.Endpoints.Reports;
using Dima.Api.Endpoints.Stripe;
using Dima.Api.Endpoints.Transactions;
using Dima.Api.Models;

namespace Dima.Api.Endpoints;

public static class Endpoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("v1");

        endpoints.MapGroup("/")
            .WithTags("Health Check")
            .MapGet("/", () => new { message = "Ok" });

        endpoints.MapGroup("/categories")
            .WithTags("Categories")
            .RequireAuthorization()
            .MapEndpoint<CreateCategoryEndpoint>()
            .MapEndpoint<UpdateCategoryEndpoint>()
            .MapEndpoint<DeleteCategoryEndpoint>()
            .MapEndpoint<GetCategoryByIdEndpoint>()
            .MapEndpoint<GetAllCategoriesEndpoint>();

        endpoints.MapGroup("/transactions")
            .WithTags("Transactions")
            .RequireAuthorization()
            .MapEndpoint<CreateTransactionEndpoint>()
            .MapEndpoint<UpdateTransactionEndpoint>()
            .MapEndpoint<DeleteTransactionEndpoint>()
            .MapEndpoint<GetTransactionByIdEndpoint>()
            .MapEndpoint<GetTransactionsByPeriod>();

        endpoints.MapGroup("/reports")
            .WithTags("Reports")
            .RequireAuthorization()
            .MapEndpoint<GetExpensesByCategoryEndpoint>()
            .MapEndpoint<GetFinancialSummaryEndpoint>()
            .MapEndpoint<GetIncomeAndExpensesEndpoint>()
            .MapEndpoint<GetIncomesByCategoryEndpoint>();

        endpoints.MapGroup("/products")
            .WithTags("Products")
            .RequireAuthorization()
            .MapEndpoint<GetAllProductsEndpoint>()
            .MapEndpoint<GetProductBySlugEndpoint>();

        endpoints.MapGroup("/vouchers")
            .WithTags("Vouchers")
            .RequireAuthorization()
            .MapEndpoint<GetVoucherByNumberEndpoint>();
        
        endpoints.MapGroup("/orders")
            .WithTags("Orders")
            .RequireAuthorization()
            .MapEndpoint<CancelOrderEndpoint>()
            .MapEndpoint<CreateOrderEndpoint>()
            .MapEndpoint<GetAllOrdersEndpoint>()
            .MapEndpoint<GetOrderByNumberEndpoint>()
            .MapEndpoint<PayOrderEndpoint>()
            .MapEndpoint<RefundOrderEndpoint>();

        endpoints.MapGroup("/payments/stripe")
            .WithTags("Payments - Stripe")
            .RequireAuthorization()
            .MapEndpoint<CreateSessionEndpoint>();
        
        endpoints.MapGroup("/identity")
            .WithTags("Identity")
            .MapIdentityApi<User>();
        
        endpoints.MapGroup("/identity")
            .WithTags("Identity")
            .MapEndpoint<LogOutEndpoint>()
            .MapEndpoint<GetRolesEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}