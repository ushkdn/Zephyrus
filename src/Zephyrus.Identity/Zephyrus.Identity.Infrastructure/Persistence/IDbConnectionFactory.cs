using Npgsql;

namespace Zephyrus.Identity.Infrastructure.Persistence;

public interface IDbConnectionFactory
{
    NpgsqlConnection CreateConnection();
}