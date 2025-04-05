using Microsoft.Data.SqlClient;

namespace CabaVS.ExpenseTracker.Persistence.Dapper;

internal sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public SqlConnection CreateConnection() => new(connectionString);
}
