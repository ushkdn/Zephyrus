using Zephyrus.Identity.Domain.Enums;
using Zephyrus.SharedKernel.Common.Entities;

namespace Zephyrus.Identity.Domain.Entities;

public class UserEntity : Entity
{
    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public UserRole Role { get; set; }

    public bool IsActive { get; set; }
}