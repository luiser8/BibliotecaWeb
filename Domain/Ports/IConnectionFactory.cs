using Microsoft.Data.SqlClient;

namespace Domain.Ports;

public interface IConnectionFactory
{
    SqlConnection CreateConnection();
}