using System.Collections;
using System.Data;

namespace Domain.Ports;

public interface IDataTableExecute
{
    public interface IDataTableExecute
    {
        Task<DataTable> ExecuteAsync(string name, Hashtable hashtable);
    }
}