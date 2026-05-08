using Zephyrus.Procurement.Domain.Enums;

namespace Zephyrus.Procurement.Domain.Entities;

public class OrderItemEntity
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid PurchaseRequestId { get; set; }

    public decimal UnitPrice { get; set; }

    public Currency Currency { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }
}
