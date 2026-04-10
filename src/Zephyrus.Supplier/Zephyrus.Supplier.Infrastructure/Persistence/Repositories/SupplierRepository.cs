using Dapper;
using Zephyrus.Supplier.Application.Interfaces;
using Zephyrus.Supplier.Domain.Entities;

namespace Zephyrus.Supplier.Infrastructure.Persistence.Repositories;

public class SupplierRepository(IDbConnectionFactory dbConnectionFactory) : ISupplierRepository
{
    public async Task<SupplierEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                name,
                contact_person AS ContactPerson,
                email,
                phone,
                is_active AS IsActive,
                date_created AS DateCreated,
                date_updated AS DateUpdated
            FROM {TableNames.Suppliers}
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<SupplierEntity>(command);
    }

    public async Task<IEnumerable<SupplierEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                name,
                contact_person AS ContactPerson,
                email,
                phone,
                is_active AS IsActive,
                date_created AS DateCreated,
                date_updated AS DateUpdated
            FROM {TableNames.Suppliers}
            ORDER BY name
            """;

        var command = new CommandDefinition(query, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<SupplierEntity>(command);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT COUNT(1) FROM {TableNames.Suppliers}
            WHERE name = @Name
            """;

        var command = new CommandDefinition(query, new { Name = name.Trim() }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(command);
    }

    public async Task AddAsync(SupplierEntity supplier, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            INSERT INTO {TableNames.Suppliers}
                (id, name, contact_person, email, phone, is_active, date_created, date_updated)
            VALUES
                (@Id, @Name, @ContactPerson, @Email, @Phone, @IsActive, @DateCreated, @DateUpdated)
            """;

        var command = new CommandDefinition(query, supplier, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }

    public async Task UpdateAsync(SupplierEntity supplier, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            UPDATE {TableNames.Suppliers} SET
                name = @Name,
                contact_person = @ContactPerson,
                email = @Email,
                phone = @Phone,
                is_active = @IsActive,
                date_updated = @DateUpdated
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, supplier, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            DELETE FROM {TableNames.Suppliers}
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }
}