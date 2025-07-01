using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Categories;

public class UpdateCategoryRequest : Request
{
    public long Id { get; set; }
    
    [MinLength(3, ErrorMessage = "Title must be more than 3 characters")]
    [MaxLength(80, ErrorMessage = "Title must be less than 80 characters")]
    public string Title { get; set; } = string.Empty;
    
    [MinLength(3, ErrorMessage = "Description must be more than 3 characters")]
    [MaxLength(255, ErrorMessage = "Description must be less than 255 characters")]
    public string? Description { get; set; }
}