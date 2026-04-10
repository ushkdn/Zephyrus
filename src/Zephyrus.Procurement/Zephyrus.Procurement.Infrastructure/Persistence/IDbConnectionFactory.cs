using Npgsql;

namespace Zephyrus.Procurement.Infrastructure.Persistence;

public interface IDbConnectionFactory
{
    NpgsqlConnection CreateConnection();
}
