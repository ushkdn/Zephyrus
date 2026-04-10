using Npgsql;

namespace Zephyrus.Notification.Infrastructure.Persistence;

public interface IDbConnectionFactory
{
    NpgsqlConnection CreateConnection();
}
