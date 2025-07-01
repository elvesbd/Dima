using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Categories;

public class CreateCategoryRequest : Request
{
    [Required(ErrorMessage = "Title is required")]
    [MinLength(3, ErrorMessage = "Title must be more than 3 characters")]
    [MaxLength(80, ErrorMessage = "Title must be less than 80 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [MinLength(3, ErrorMessage = "Description must be more than 3 characters")]
    [MaxLength(255, ErrorMessage = "Description must be less than 255 characters")]
    public string Description { get; set; } = string.Empty;
}