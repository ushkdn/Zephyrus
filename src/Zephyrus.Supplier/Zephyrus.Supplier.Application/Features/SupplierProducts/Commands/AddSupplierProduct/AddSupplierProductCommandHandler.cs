using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.SharedKernel.Common;
using Zephyrus.Supplier.Application.Interfaces;
using Zephyrus.Supplier.Domain.Entities;

namespace Zephyrus.Supplier.Application.Features.SupplierProducts.Commands.AddSupplierProduct;

public class AddSupplierProductCommandHandler(
    ISupplierRepository supplierRepository,
    ISupplierProductRepository supplierProductRepository,
    IProductExistenceChecker productExistenceChecker,
    ILogger<AddSupplierProductCommandHandler> logger)
    : IRequestHandler<AddSupplierProductCommandRequest, HandlerResponse<AddSupplierProductCommandResponse>>
{
    public async Task<HandlerResponse<AddSupplierProductCommandResponse>> Handle(AddSupplierProductCommandRequest request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken);

        if (supplier is null)
        {
            logger.LogWarning("Supplier {SupplierId} not found", request.SupplierId);
            return new HandlerResponse<AddSupplierProductCommandResponse>(null, $"Supplier with id: {request.SupplierId} not found.", false);
        }

        if (!await productExistenceChecker.ExistsAsync(request.ProductId, cancellationToken))
        {
            logger.LogWarning("Product {ProductId} not found in Catalog", request.ProductId);
            return new HandlerResponse<AddSupplierProductCommandResponse>(null, $"Product with id: {request.ProductId} not found in Catalog.", false);
        }

        if (await supplierProductRepository.ExistsBySupplierAndProductAsync(request.SupplierId, request.ProductId, cancellationToken))
        {
            logger.LogWarning("Product {ProductId} is already linked to supplier {SupplierId}", request.ProductId, request.SupplierId);
            return new HandlerResponse<AddSupplierProductCommandResponse>(null, $"Product with id: {request.ProductId} is already linked to this supplier.", false);
        }

        var supplierProduct = new SupplierProductEntity
        {
            Id = Guid.NewGuid(),
            SupplierId = request.SupplierId,
            ProductId = request.ProductId,
            Price = request.Price,
            Currency = request.Currency.Trim().ToUpper(),
            IsAvailable = true,
            DateUpdated = DateTime.UtcNow
        };

        await supplierProductRepository.AddAsync(supplierProduct, cancellationToken);

        logger.LogInformation("Product {ProductId} added to supplier {SupplierId} with price {Price} {Currency}", supplierProduct.ProductId, supplierProduct.SupplierId, supplierProduct.Price, supplierProduct.Currency);

        return new HandlerResponse<AddSupplierProductCommandResponse>(
            new AddSupplierProductCommandResponse(supplierProduct.Id, supplierProduct.SupplierId, supplierProduct.ProductId, supplierProduct.Price, supplierProduct.Currency),
            $"Product added to supplier successfully with id: {supplierProduct.Id}",
            true);
    }
}
