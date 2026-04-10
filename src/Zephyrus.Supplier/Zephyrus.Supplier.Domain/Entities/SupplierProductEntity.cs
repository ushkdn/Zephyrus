namespace Zephyrus.Supplier.Domain.Entities;

public class SupplierProductEntity
{
    public Guid Id { get; set; }

    public Guid SupplierId { get; set; }

    public Guid ProductId { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = string.Empty;

    public bool IsAvailable { get; set; }

    public DateTime DateUpdated { get; set; }
}