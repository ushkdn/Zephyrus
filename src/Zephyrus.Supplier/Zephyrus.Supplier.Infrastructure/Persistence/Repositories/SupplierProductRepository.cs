using Dapper;
using Zephyrus.Supplier.Application.Interfaces;
using Zephyrus.Supplier.Domain.Entities;

namespace Zephyrus.Supplier.Infrastructure.Persistence.Repositories;

public class SupplierProductRepository(IDbConnectionFactory dbConnectionFactory) : ISupplierProductRepository
{
    public async Task<SupplierProductEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                supplier_id AS SupplierId,
                product_id AS ProductId,
                price,
                currency,
                is_available AS IsAvailable,
                date_updated AS DateUpdated
            FROM {TableNames.SupplierProducts}
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<SupplierProductEntity>(command);
    }

    public async Task<IEnumerable<SupplierProductEntity>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                supplier_id AS SupplierId,
                product_id AS ProductId,
                price,
                currency,
                is_available AS IsAvailable,
                date_updated AS DateUpdated
            FROM {TableNames.SupplierProducts}
            WHERE supplier_id = @SupplierId
            ORDER BY product_id
            """;

        var command = new CommandDefinition(query, new { SupplierId = supplierId }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<SupplierProductEntity>(command);
    }

    public async Task<bool> ExistsBySupplierAndProductAsync(Guid supplierId, Guid productId, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT COUNT(1) FROM {TableNames.SupplierProducts}
            WHERE supplier_id = @SupplierId AND product_id = @ProductId
            """;

        var command = new CommandDefinition(query, new { SupplierId = supplierId, ProductId = productId }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(command);
    }

    public async Task AddAsync(SupplierProductEntity supplierProduct, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            INSERT INTO {TableNames.SupplierProducts}
                (id, supplier_id, product_id, price, currency, is_available, date_updated)
            VALUES
                (@Id, @SupplierId, @ProductId, @Price, @Currency, @IsAvailable, @DateUpdated)
            """;

        var command = new CommandDefinition(query, supplierProduct, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }

    public async Task UpdateAsync(SupplierProductEntity supplierProduct, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            UPDATE {TableNames.SupplierProducts} SET
                price = @Price,
                currency = @Currency,
                is_available = @IsAvailable,
                date_updated = @DateUpdated
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, supplierProduct, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            DELETE FROM {TableNames.SupplierProducts}
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }
}