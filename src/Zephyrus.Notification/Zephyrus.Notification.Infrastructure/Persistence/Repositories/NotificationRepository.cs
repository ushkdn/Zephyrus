using Dapper;
using Zephyrus.Notification.Application.Interfaces;
using Zephyrus.Notification.Domain.Entities;

namespace Zephyrus.Notification.Infrastructure.Persistence.Repositories;

public class NotificationRepository(IDbConnectionFactory dbConnectionFactory) : INotificationRepository
{
    public async Task<NotificationEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                recipient_id AS RecipientId,
                title,
                message,
                type,
                is_read AS IsRead,
                date_created AS DateCreated
            FROM {TableNames.Notifications}
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<NotificationEntity>(command);
    }

    public async Task<IEnumerable<NotificationEntity>> GetByRecipientIdAsync(Guid recipientId, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            SELECT
                id,
                recipient_id AS RecipientId,
                title,
                message,
                type,
                is_read AS IsRead,
                date_created AS DateCreated
            FROM {TableNames.Notifications}
            WHERE recipient_id = @RecipientId
            ORDER BY date_created DESC
            """;

        var command = new CommandDefinition(query, new { RecipientId = recipientId }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<NotificationEntity>(command);
    }

    public async Task AddAsync(NotificationEntity notification, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            INSERT INTO {TableNames.Notifications}
                (id, recipient_id, title, message, type, is_read, date_created)
            VALUES
                (@Id, @RecipientId, @Title, @Message, @Type, @IsRead, @DateCreated)
            """;

        var command = new CommandDefinition(query, notification, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }

    public async Task MarkAsReadAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query =
            $"""
            UPDATE {TableNames.Notifications} SET
                is_read = true
            WHERE id = @Id
            """;

        var command = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);

        await using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(command);
    }
}
