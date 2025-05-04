namespace SampleApp.Application.Models;

public class OrderCreateModel
{
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}
