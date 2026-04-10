using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommandRequest(
    Guid Id,
    string Name,
    Guid? ParentId
) : IRequest<HandlerResponse<UpdateCategoryCommandResponse>>;