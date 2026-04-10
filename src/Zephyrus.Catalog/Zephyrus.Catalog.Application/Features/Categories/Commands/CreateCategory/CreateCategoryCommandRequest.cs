using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommandRequest(
    string Name,
    Guid? ParentId
) : IRequest<HandlerResponse<CreateCategoryCommandResponse>>;