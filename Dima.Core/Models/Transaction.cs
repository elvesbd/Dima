using Dima.Core.Enums;

namespace Dima.Core.Models;

public class Transaction
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime? PaidOrReceived { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ETransactionType Type { get; set; } = ETransactionType.Withdraw;
    
    public long CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public string UserId { get; set; } = string.Empty;
}