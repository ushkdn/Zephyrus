using DbUp;
using System.Reflection;

namespace Zephyrus.SharedKernel.Common.Database;

public static class MigrationRunner
{
    public static void Run(string connectionString, Assembly migrationsAssembly)
    {
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(migrationsAssembly)
            .WithTransactionPerScript()
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
            throw new Exception($"Migration failed: {result.Error.Message}", result.Error);
    }
}