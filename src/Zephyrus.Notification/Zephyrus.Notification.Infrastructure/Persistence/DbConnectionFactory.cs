using Npgsql;

namespace Zephyrus.Notification.Infrastructure.Persistence;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public NpgsqlConnection CreateConnection() => new(connectionString);
}
