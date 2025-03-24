using Microsoft.Data.SqlClient;

namespace CabaVS.ExpenseTracker.Persistence;

internal sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public SqlConnection CreateConnection() => new(connectionString);
}
