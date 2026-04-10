using Npgsql;

namespace Zephyrus.Catalog.Infrastructure.Persistence;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public NpgsqlConnection CreateConnection() => new(connectionString);
}