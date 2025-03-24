using Microsoft.Data.SqlClient;

namespace CabaVS.ExpenseTracker.Persistence;

internal interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}
