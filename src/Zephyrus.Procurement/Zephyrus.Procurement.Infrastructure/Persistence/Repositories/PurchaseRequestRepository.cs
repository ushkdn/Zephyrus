using Dapper;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.Procurement.Domain.Entities;

namespace Zephyrus.Procurement.Infrastructure.Persistence.Repositories;

public class PurchaseRequestRepository(IDbConnectionFactory dbConnectionFactory) : IPurchaseRequestRepository
{
    public async Task<PurchaseRequestEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                product_id AS ProductId,
                quantity,
                requested_by AS RequestedBy,
                status,
                comment,
                date_created AS DateCreated,
                date_updated AS DateUpdated
            FROM {TableNames.PurchaseRequests}
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<PurchaseRequestEntity>(command);
    }

    public async Task<IEnumerable<PurchaseRequestEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                product_id AS ProductId,
                quantity,
                requested_by AS RequestedBy,
                status,
                comment,
                date_created AS DateCreated,
                date_updated AS DateUpdated
            FROM {TableNames.PurchaseRequests}
            ORDER BY date_created DESC
            """;

        var command = new CommandDefinition(query, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<PurchaseRequestEntity>(command);
    }

    public async Task<IEnumerable<PurchaseRequestEntity>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                product_id AS ProductId,
                quantity,
                requested_by AS RequestedBy,
                status,
                comment,
                date_created AS DateCreated,
                date_updated AS DateUpdated
            FROM {TableNames.PurchaseRequests}
            WHERE requested_by = @UserId
            ORDER BY date_created DESC
            """;

        var command = new CommandDefinition(query, new { UserId = userId }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<PurchaseRequestEntity>(command);
    }

    public async Task AddAsync(PurchaseRequestEntity purchaseRequest, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            INSERT INTO {TableNames.PurchaseRequests}
                (id, product_id, quantity, requested_by, status, comment, date_created, date_updated)
            VALUES
                (@Id, @ProductId, @Quantity, @RequestedBy, @Status, @Comment, @DateCreated, @DateUpdated)
            """;

        var param = new
        {
            purchaseRequest.Id,
            purchaseRequest.ProductId,
            purchaseRequest.Quantity,
            purchaseRequest.RequestedBy,
            Status = purchaseRequest.Status.ToString(),
            purchaseRequest.Comment,
            purchaseRequest.DateCreated,
            purchaseRequest.DateUpdated
        };

        var command = new CommandDefinition(query, param, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }

    public async Task UpdateAsync(PurchaseRequestEntity purchaseRequest, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            UPDATE {TableNames.PurchaseRequests} SET
                status = @Status,
                comment = @Comment,
                date_updated = @DateUpdated
            WHERE id = @Id
            """;

        var param = new
        {
            Status = purchaseRequest.Status.ToString(),
            purchaseRequest.Comment,
            purchaseRequest.DateUpdated,
            purchaseRequest.Id
        };
        var command = new CommandDefinition(query, param, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }
}
