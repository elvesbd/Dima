using Dima.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Transactions;

public class UpdateTransactionRequest : Request
{
    public long Id { get; set; }
    
    [Required(ErrorMessage = "Title is required")]
    [StringLength(80, ErrorMessage = "Title must be at most 100 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Type is required")]
    [EnumDataType(typeof(ETransactionType), ErrorMessage = "Invalid transaction type")]
    public ETransactionType Type { get; set; }
    
    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; set; }
    
    [Required(ErrorMessage = "PaidOrReceived is required")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
    public DateTime? PaidOrReceived { get; set; }

    [Required(ErrorMessage = "CategoryId is required")]
    [Range(1, long.MaxValue, ErrorMessage = "CategoryId must be a positive number")]
    public long CategoryId { get; set; }
}