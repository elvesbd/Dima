namespace Dima.Core.Requests.Orders;

public class PayOrderRequest : Request
{
    public string Number { get; set; } = string.Empty;
    public string ExternalReference { get; set; }  = string.Empty;
}