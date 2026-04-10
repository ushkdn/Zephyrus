using Zephyrus.Procurement.Domain.Enums;

namespace Zephyrus.Procurement.Domain.Entities;

public class PurchaseRequestEntity
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public decimal Quantity { get; set; }

    public string Unit { get; set; } = string.Empty;

    public Guid RequestedBy { get; set; }

    public PurchaseRequestStatus Status { get; set; }

    public string? Comment { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }
}
