using Microsoft.Data.SqlClient;

namespace CabaVS.ExpenseTracker.Persistence.Dapper;

internal interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}
