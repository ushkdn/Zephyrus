using Dapper;
using Zephyrus.Identity.Application.Interfaces;
using Zephyrus.Identity.Domain.Entities;

namespace Zephyrus.Identity.Infrastructure.Persistence.Repositories;

public class UserRepository(IDbConnectionFactory dbConnectionFactory) : IUserRepository
{
    public async Task AddAsync(UserEntity user, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            INSERT INTO {TableNames.Users}
            (id, email, password, first_name, middle_name, last_name, role, is_active, date_created, date_updated)
            VALUES
            (@Id, @Email, @Password, @FirstName, @MiddleName, @LastName, @Role, @IsActive, @DateCreated, @DateUpdated)
            """;

        var command = new CommandDefinition(
            commandText: query,
            parameters: user,
            cancellationToken: cancellationToken
            );

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }

    public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
            id,
            email,
            password,
            first_name AS FirstName,
            middle_name AS MiddleName,
            last_name AS LastName,
            role,
            is_active AS IsActive,
            date_created AS DateCreated,
            date_updated AS DateUpdated
            FROM {TableNames.Users}
            WHERE email = @Email
            """;

        var command = new CommandDefinition(
            commandText: query,
            parameters: new { Email = email.ToLowerInvariant().Trim() },
            cancellationToken: cancellationToken
            );

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<UserEntity>(command);
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
            id,
            email,
            password,
            first_name AS FirstName,
            middle_name AS MiddleName,
            last_name AS LastName,
            role,
            is_active AS IsActive,
            date_created AS DateCreated,
            date_updated AS DateUpdated
            FROM {TableNames.Users}
            WHERE id = @Id
            """;

        var command = new CommandDefinition(
            commandText: query,
            parameters: new { Id = id },
            cancellationToken: cancellationToken
            );

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<UserEntity>(command);
    }

    public async Task<bool> IsExistsByEmailAsync(string email, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT COUNT(1) FROM {TableNames.Users}
            WHERE email = @Email
            """;

        var command = new CommandDefinition(
            commandText: query,
            parameters: new { Email = email.ToLowerInvariant().Trim() },
            cancellationToken: cancellationToken
            );

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(command);
    }

    public async Task UpdateAsync(UserEntity user, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            UPDATE {TableNames.Users} SET
                email = @Email,
                password = @Password,
                first_name = @FirstName,
                middle_name = @MiddleName,
                last_name = @LastName,
                role = @Role,
                is_active = @IsActive,
                date_updated = @DateUpdated
            WHERE id = @Id
            """;

        var command = new CommandDefinition(
            commandText: query,
            parameters: user,
            cancellationToken: cancellationToken
            );

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }
}