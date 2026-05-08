using Dapper;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.Procurement.Domain.Entities;

namespace Zephyrus.Procurement.Infrastructure.Persistence.Repositories;

public class OrderRepository(IDbConnectionFactory dbConnectionFactory) : IOrderRepository
{
    public async Task<OrderEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                supplier_id AS SupplierId,
                total_price AS TotalPrice,
                status,
                created_by AS CreatedBy,
                date_created AS DateCreated,
                date_updated AS DateUpdated
            FROM {TableNames.Orders}
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<OrderEntity>(command);
    }

    public async Task<IEnumerable<OrderEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                supplier_id AS SupplierId,
                total_price AS TotalPrice,
                status,
                created_by AS CreatedBy,
                date_created AS DateCreated,
                date_updated AS DateUpdated
            FROM {TableNames.Orders}
            ORDER BY date_created DESC
            """;

        var command = new CommandDefinition(query, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<OrderEntity>(command);
    }

    public async Task<bool> ExistsByPurchaseRequestIdAsync(Guid purchaseRequestId, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT COUNT(1) FROM {TableNames.OrderItems}
            WHERE purchase_request_id = @PurchaseRequestId
            """;

        var command = new CommandDefinition(query, new { PurchaseRequestId = purchaseRequestId }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(command);
    }

    public async Task<IEnumerable<OrderItemEntity>> GetItemsByOrderIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                order_id AS OrderId,
                purchase_request_id AS PurchaseRequestId,
                unit_price AS UnitPrice,
                currency,
                total_price AS TotalPrice,
                date_created AS DateCreated,
                date_updated AS DateUpdated
            FROM {TableNames.OrderItems}
            WHERE order_id = @OrderId
            """;

        var command = new CommandDefinition(query, new { OrderId = orderId }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<OrderItemEntity>(command);
    }

    public async Task<IEnumerable<OrderItemEntity>> GetItemsByOrderIdsAsync(IEnumerable<Guid> orderIds, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                order_id AS OrderId,
                purchase_request_id AS PurchaseRequestId,
                unit_price AS UnitPrice,
                currency,
                total_price AS TotalPrice,
                date_created AS DateCreated,
                date_updated AS DateUpdated
            FROM {TableNames.OrderItems}
            WHERE order_id = ANY(@OrderIds)
            """;

        var command = new CommandDefinition(query, new { OrderIds = orderIds.ToArray() }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<OrderItemEntity>(command);
    }

    public async Task AddAsync(OrderEntity order, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            INSERT INTO {TableNames.Orders}
                (id, supplier_id, total_price, status, created_by, date_created, date_updated)
            VALUES
                (@Id, @SupplierId, @TotalPrice, @Status, @CreatedBy, @DateCreated, @DateUpdated)
            """;

        var command = new CommandDefinition(query, order, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }

    public async Task AddOrderItemAsync(OrderItemEntity item, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            INSERT INTO {TableNames.OrderItems}
                (id, order_id, purchase_request_id, unit_price, currency, total_price, date_created, date_updated)
            VALUES
                (@Id, @OrderId, @PurchaseRequestId, @UnitPrice, @Currency, @TotalPrice, @DateCreated, @DateUpdated)
            """;

        var param = new
        {
            item.Id,
            item.OrderId,
            item.PurchaseRequestId,
            item.UnitPrice,
            Currency = item.Currency.ToString(),
            item.TotalPrice,
            item.DateCreated,
            item.DateUpdated
        };
        var command = new CommandDefinition(query, param, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }

    public async Task UpdateAsync(OrderEntity order, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            UPDATE {TableNames.Orders} SET
                status = @Status,
                date_updated = @DateUpdated
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, order, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }
}
