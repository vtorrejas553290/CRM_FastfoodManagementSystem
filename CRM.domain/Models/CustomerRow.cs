namespace CRM.domain.Models;

public class CustomerRow
{
    public int CustomerId { get; set; }
    public string CustomerCode { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string ContactNumber { get; set; } = "";
    public string EmailAddress { get; set; } = "";
    public string Address { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}