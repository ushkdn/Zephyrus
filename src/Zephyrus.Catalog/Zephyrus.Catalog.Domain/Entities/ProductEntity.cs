namespace Zephyrus.Catalog.Domain.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Unit { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }

    public bool IsActive { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }
}