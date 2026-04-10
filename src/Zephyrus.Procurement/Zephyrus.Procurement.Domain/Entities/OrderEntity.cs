using Zephyrus.Procurement.Domain.Enums;

namespace Zephyrus.Procurement.Domain.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }

    public Guid PurchaseRequestId { get; set; }

    public Guid SupplierId { get; set; }

    public Guid ProductId { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public string Currency { get; set; } = string.Empty;

    public decimal TotalPrice { get; set; }

    public OrderStatus Status { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }
}
