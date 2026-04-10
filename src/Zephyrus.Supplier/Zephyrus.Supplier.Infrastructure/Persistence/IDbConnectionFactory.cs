using Npgsql;

namespace Zephyrus.Supplier.Infrastructure.Persistence;

public interface IDbConnectionFactory
{
    NpgsqlConnection CreateConnection();
}