using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommandRequest(Guid Id) : IRequest<HandlerResponse<DeleteCategoryCommandResponse>>;