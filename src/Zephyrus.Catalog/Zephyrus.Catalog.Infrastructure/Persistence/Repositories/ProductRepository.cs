using Dapper;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.Catalog.Domain.Entities;

namespace Zephyrus.Catalog.Infrastructure.Persistence.Repositories;

public class ProductRepository(IDbConnectionFactory dbConnectionFactory) : IProductRepository
{
    public async Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                name,
                description,
                unit,
                category_id AS CategoryId,
                is_active AS IsActive,
                date_created AS DateCreated,
                date_updated AS DateUpdated
            FROM {TableNames.Products}
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<ProductEntity>(command);
    }

    public async Task<IEnumerable<ProductEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                name,
                description,
                unit,
                category_id AS CategoryId,
                is_active AS IsActive,
                date_created AS DateCreated,
                date_updated AS DateUpdated
            FROM {TableNames.Products}
            ORDER BY name
            """;

        var command = new CommandDefinition(query, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<ProductEntity>(command);
    }

    public async Task<IEnumerable<ProductEntity>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                name,
                description,
                unit,
                category_id AS CategoryId,
                is_active AS IsActive,
                date_created AS DateCreated,
                date_updated AS DateUpdated
            FROM {TableNames.Products}
            WHERE category_id = @CategoryId
            ORDER BY name
            """;

        var command = new CommandDefinition(query, new { CategoryId = categoryId }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<ProductEntity>(command);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT COUNT(1) FROM {TableNames.Products}
            WHERE name = @Name
            """;

        var command = new CommandDefinition(query, new { Name = name.Trim() }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(command);
    }

    public async Task AddAsync(ProductEntity product, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            INSERT INTO {TableNames.Products}
                (id, name, description, unit, category_id, is_active, date_created, date_updated)
            VALUES
                (@Id, @Name, @Description, @Unit, @CategoryId, @IsActive, @DateCreated, @DateUpdated)
            """;

        var command = new CommandDefinition(query, product, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }

    public async Task UpdateAsync(ProductEntity product, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            UPDATE {TableNames.Products} SET
                name = @Name,
                description = @Description,
                unit = @Unit,
                category_id = @CategoryId,
                is_active = @IsActive,
                date_updated = @DateUpdated
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, product, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            DELETE FROM {TableNames.Products}
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }
}