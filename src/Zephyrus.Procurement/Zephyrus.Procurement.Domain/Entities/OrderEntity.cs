using Zephyrus.Procurement.Domain.Enums;

namespace Zephyrus.Procurement.Domain.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }

    public Guid SupplierId { get; set; }

    public decimal TotalPrice { get; set; }

    public OrderStatus Status { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }
}
