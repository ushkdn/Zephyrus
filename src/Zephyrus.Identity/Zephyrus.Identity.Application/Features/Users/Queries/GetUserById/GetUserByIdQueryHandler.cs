using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Identity.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Identity.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(
    IUserRepository userRepository,
    ILogger<GetUserByIdQueryHandler> logger
    ) : IRequestHandler<GetUserByIdQueryRequest, HandlerResponse<GetUserByIdQueryResponse>>
{
    public async Task<HandlerResponse<GetUserByIdQueryResponse>> Handle(GetUserByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var storedUser = await userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (storedUser is null)
        {
            logger.LogWarning("User with id: {id} not found", request.Id);
            return new HandlerResponse<GetUserByIdQueryResponse>(null, $"User with id: {request.Id} not found.", false);
        }

        logger.LogInformation("User with id: {id} successfully retrieved", request.Id);

        return new HandlerResponse<GetUserByIdQueryResponse>(
            new GetUserByIdQueryResponse(
                storedUser.Id,
                storedUser.Email,
                storedUser.FirstName,
                storedUser.MiddleName,
                storedUser.LastName,
                storedUser.Role,
                storedUser.IsActive,
                storedUser.DateCreated,
                storedUser.DateUpdated
            ),
            $"User with id: {request.Id} successfully retrieved",
            true);

    }
}