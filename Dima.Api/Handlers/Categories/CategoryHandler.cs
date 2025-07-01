using Dima.Api.Data;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Core.Responses;
using Dima.Core.Requests.Categories;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers.Categories;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {
            var category = new Category
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description
            };
            
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            
            return new Response<Category?>(
                category,
                201,
                "Category created");
        }
        catch
        {
            return new Response<Category?>(
                null,
                500,
                "Not able to create category");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category is null)
                return new Response<Category?>(
                    null,
                    404,
                    "Category not found");

            category.Title = request.Title;
            category.Description = request.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();
            
            return new Response<Category?>(category, message: "Category updated");
        }
        catch
        {
            return new Response<Category?>(
                null,
                500,
                "Not able to update category");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category == null)
                return new Response<Category?>(
                    null,
                    404,
                    "Category not found");

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(
                category,
                message: "Category deleted");
        }
        catch
        {
            return new Response<Category?>(
                null,
                500,
                "Not able to delete category");
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return category is null
                ? new Response<Category?>(
                    null,
                    404,
                    "Category not found")
                : new Response<Category?>(category);
        }
        catch
        {
            return new Response<Category?>(
                null,
                500,
                "Not able to get category by id");
        }
    }

    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
    {
        try
        {
            var query = context
                .Categories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Title);

            var categories = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Category>>(
                categories,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Category>>(
                null,
                500,
                "Not able to get categories");
        }
    }
}