using Npgsql;

namespace Zephyrus.Identity.Infrastructure.Persistence;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public NpgsqlConnection CreateConnection() => new(connectionString);
}