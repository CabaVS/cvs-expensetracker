using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence;

internal sealed class SqlConnectionFactory(IConfiguration configuration)
{
    public IDbConnection Create() => new SqlConnection(configuration.GetConnectionString("SqlDatabase"));
}