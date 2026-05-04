using Zephyrus.Identity.Domain.Enums;

namespace Zephyrus.Identity.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQueryResponse(
    Guid Id,
    string Email,
    string FirstName,
    string MiddleName,
    string LastName,
    UserRole Role,
    bool IsActive,
    DateTime DateCreated,
    DateTime DateUpdated
    );