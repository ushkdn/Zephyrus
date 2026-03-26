using MediatR;
using Zephyrus.Identity.Application.Interfaces;
using Zephyrus.Identity.Domain.Entities;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.SignUp;

public class SignUpCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher
    ) : IRequestHandler<SignUpCommandRequest, HandlerResponse<SignUpCommandResponse>>
{
    public async Task<HandlerResponse<SignUpCommandResponse>> Handle(SignUpCommandRequest request, CancellationToken cancellationToken)
    {
        if (await userRepository.IsExistsByEmailAsync(request.Email, cancellationToken))
        {
            return new HandlerResponse<SignUpCommandResponse>(null, $"User with email: {request.Email} is already exists.", false);
        }

        var userToStore = new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = request.Email.Trim(),
            FirstName = request.FirstName.Trim(),
            MiddleName = request.MiddleName.Trim(),
            LastName = request.LastName.Trim(),
            Password = passwordHasher.Hash(request.Password).Trim(),
            Role = request.Role,
            IsActive = true,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };

        await userRepository.AddAsync(userToStore, cancellationToken);

        return new HandlerResponse<SignUpCommandResponse>(
            new SignUpCommandResponse(userToStore.Id, request.Email),
            $"User created successfully with id: {userToStore.Id}",
            true
            );
    }
}