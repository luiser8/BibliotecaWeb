using Domain.Ports;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Configurations;

public class ConnectionFactory(string connectionString) : IConnectionFactory
{
    public SqlConnection CreateConnection() => new(connectionString);
}
