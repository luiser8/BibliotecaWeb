using System.Data.Common;
using Domain.Ports;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Configurations;

public class ConnectionFactory(string connectionString) : IConnectionFactory
{
    DbConnection IConnectionFactory.CreateConnection()
    {
        return new SqlConnection(connectionString);
    }
}
