using Dima.Core;
using Dima.Api.Data;
using Dima.Api.Models;
using Dima.Core.Handlers;
using Dima.Api.Handlers.Categories;
using Dima.Api.Handlers.Reports;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dima.Api.Handlers.Transactions;

namespace Dima.Api.Common.Api;

public static class BuilderExtension
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.ConnectionString = builder
            .Configuration
            .GetConnectionString("DefaultConnection") ?? string.Empty;

        Configuration.BackEndUrl = builder.Configuration
            .GetValue<string>("BackEndUrl") ?? string.Empty;
        Configuration.FrontEndUrl = builder.Configuration
            .GetValue<string>("FrontEndUrl") ?? string.Empty;
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x => x.CustomSchemaIds(n => n.FullName));
    }

    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        builder
            .Services
            .AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies();
        builder.Services.AddAuthorization();
    }

    public static void AddDataContexts(this WebApplicationBuilder builder)
    {
        builder
            .Services
            .AddDbContext<AppDbContext>(x =>
                x.UseSqlServer(Configuration.ConnectionString));
        
        builder
            .Services
            .AddIdentityCore<User>()
            .AddRoles<IdentityRole<long>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder
            .Services
            .AddCors(options => options.AddPolicy(
                ApiConfiguration.CorsPolicyName,
                policy => policy
                    .WithOrigins([
                        Configuration.BackEndUrl,
                        Configuration.FrontEndUrl
                    ])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                ));
    }
    
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IReportsHandler, ReportsHandler>();
        builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
        builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
    }
}