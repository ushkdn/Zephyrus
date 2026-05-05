using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.ForgotPassword;

public record ForgotPasswordCommandRequest(string Email) : IRequest<HandlerResponse<Guid?>>;