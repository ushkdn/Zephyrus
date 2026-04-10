using Npgsql;

namespace Zephyrus.Procurement.Infrastructure.Persistence;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public NpgsqlConnection CreateConnection() => new(connectionString);
}
