using Dapper;
using Zephyrus.Identity.Application.Interfaces;
using Zephyrus.Identity.Domain.Entities;

namespace Zephyrus.Identity.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(IDbConnectionFactory dbConnectionFactory) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshTokenEntity refreshToken, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            INSERT INTO {TableNames.RefreshTokens}
            (id, user_id, token, date_expires, date_created, date_updated)
            VALUES
            (@Id, @UserId, @Token, @DateExpires, @DateCreated, @DateUpdated)
            """;

        var command = new CommandDefinition(
            commandText: query,
            parameters: refreshToken,
            cancellationToken: cancellationToken
            );

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }

    public async Task<RefreshTokenEntity?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
            id,
            user_id AS UserId,
            token,
            date_expires AS DateExpires,
            date_created AS DateCreated,
            date_updated AS DateUpdated
            FROM {TableNames.RefreshTokens}
            WHERE token = @Token
            """;

        var command = new CommandDefinition(
            commandText: query,
            parameters: new { Token = token },
            cancellationToken: cancellationToken
            );

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<RefreshTokenEntity>(command);
    }

    public async Task UpdateAsync(RefreshTokenEntity refreshToken, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            UPDATE {TableNames.RefreshTokens} SET
                token = @Token,
                date_expires = @DateExpires,
                date_updated = @DateUpdated
            WHERE id = @Id
            """;

        var command = new CommandDefinition(
            commandText: query,
            parameters: refreshToken,
            cancellationToken: cancellationToken
            );

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }
}