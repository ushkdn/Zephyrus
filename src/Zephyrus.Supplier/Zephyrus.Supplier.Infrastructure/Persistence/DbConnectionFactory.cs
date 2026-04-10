using Npgsql;

namespace Zephyrus.Supplier.Infrastructure.Persistence;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public NpgsqlConnection CreateConnection() => new(connectionString);
}