using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zephyrus.Catalog.Application.Features.Categories.Commands.CreateCategory;
using Zephyrus.Catalog.Application.Features.Categories.Commands.DeleteCategory;
using Zephyrus.Catalog.Application.Features.Categories.Commands.UpdateCategory;
using Zephyrus.Catalog.Application.Features.Categories.Queries.GetAllCategories;
using Zephyrus.Catalog.Application.Features.Categories.Queries.GetCategoryById;

namespace Zephyrus.Catalog.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/categories")]
public class CategoriesController(ISender sender, ILogger<CategoriesController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllCategoriesQueryRequest(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCategoryByIdQueryRequest(id), cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Category {CategoryId} not found", id);
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Failed to create category '{Name}' — {Message}", request.Name, result.Message);
            return BadRequest(result);
        }

        logger.LogInformation("Category '{Name}' created", request.Name);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request with { Id = id }, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Failed to update category {CategoryId} — {Message}", id, result.Message);
            return BadRequest(result);
        }

        logger.LogInformation("Category {CategoryId} updated", id);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteCategoryCommandRequest(id), cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Failed to delete category {CategoryId} — {Message}", id, result.Message);
            return NotFound(result);
        }

        logger.LogInformation("Category {CategoryId} deleted", id);
        return Ok(result);
    }
}
