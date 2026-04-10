using Dapper;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.Catalog.Domain.Entities;

namespace Zephyrus.Catalog.Infrastructure.Persistence.Repositories;

public class CategoryRepository(IDbConnectionFactory dbConnectionFactory) : ICategoryRepository
{
    public async Task<CategoryEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                name,
                parent_id AS ParentId,
                date_created AS DateCreated,
                date_updated AS DateUpdated
            FROM {TableNames.Categories}
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<CategoryEntity>(command);
    }

    public async Task<IEnumerable<CategoryEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                name,
                parent_id AS ParentId,
                date_created AS DateCreated,
                date_updated AS DateUpdated
            FROM {TableNames.Categories}
            ORDER BY name
            """;

        var command = new CommandDefinition(query, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<CategoryEntity>(command);
    }

    public async Task<bool> IsExistsByNameAsync(string name, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT COUNT(1) FROM {TableNames.Categories}
            WHERE name = @Name
            """;

        var command = new CommandDefinition(query, new { Name = name.Trim() }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(command);
    }

    public async Task AddAsync(CategoryEntity category, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            INSERT INTO {TableNames.Categories}
                (id, name, parent_id, date_created, date_updated)
            VALUES
                (@Id, @Name, @ParentId, @DateCreated, @DateUpdated)
            """;

        var command = new CommandDefinition(query, category, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }

    public async Task UpdateAsync(CategoryEntity category, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            UPDATE {TableNames.Categories} SET
                name = @Name,
                parent_id = @ParentId,
                date_updated = @DateUpdated
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, category, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            DELETE FROM {TableNames.Categories}
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }
}