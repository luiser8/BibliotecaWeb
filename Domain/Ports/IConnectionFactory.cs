using System.Data.Common;

namespace Domain.Ports;

public interface IConnectionFactory
{
    DbConnection CreateConnection();
}