using EntityAxis.MediatR.Commands;
using SampleApp.Domain;

namespace SampleApp.Application.Models;

public class OrderUpdateModel : IUpdateCommandModel<Order, string>
{
    public string Id { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}
