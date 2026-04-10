using Npgsql;

namespace Zephyrus.Catalog.Infrastructure.Persistence;

public interface IDbConnectionFactory
{
    NpgsqlConnection CreateConnection();
}