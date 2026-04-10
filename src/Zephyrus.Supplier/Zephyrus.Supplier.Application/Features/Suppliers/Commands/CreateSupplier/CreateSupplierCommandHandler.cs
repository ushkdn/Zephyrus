using MediatR;
using Zephyrus.SharedKernel.Common;
using Zephyrus.Supplier.Application.Interfaces;
using Zephyrus.Supplier.Domain.Entities;

namespace Zephyrus.Supplier.Application.Features.Suppliers.Commands.CreateSupplier;

public class CreateSupplierCommandHandler(ISupplierRepository supplierRepository)
    : IRequestHandler<CreateSupplierCommandRequest, HandlerResponse<CreateSupplierCommandResponse>>
{
    public async Task<HandlerResponse<CreateSupplierCommandResponse>> Handle(CreateSupplierCommandRequest request, CancellationToken cancellationToken)
    {
        if (await supplierRepository.ExistsByNameAsync(request.Name, cancellationToken))
            return new HandlerResponse<CreateSupplierCommandResponse>(null, $"Supplier '{request.Name}' already exists.", false);

        var supplier = new SupplierEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            ContactPerson = request.ContactPerson.Trim(),
            Email = request.Email.Trim(),
            Phone = request.Phone.Trim(),
            IsActive = true,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };

        await supplierRepository.AddAsync(supplier, cancellationToken);

        return new HandlerResponse<CreateSupplierCommandResponse>(
            new CreateSupplierCommandResponse(supplier.Id, supplier.Name, supplier.Email),
            $"Supplier created successfully with id: {supplier.Id}",
            true);
    }
}