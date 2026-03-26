using Zephyrus.Identity.Domain.Enums;

namespace Zephyrus.Identity.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string MiddleName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public bool IsActive { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }
}