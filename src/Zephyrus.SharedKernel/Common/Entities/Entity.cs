namespace Zephyrus.SharedKernel.Common.Entities;

public abstract class Entity : EntityBase
{
    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }
}