using System.Collections;
using System.Data;
using System.Data.Common;
using Domain.Ports;

namespace Infrastructure.Configurations;

public class DataTableExecute : IDataTableExecute, IDisposable
{
    public bool ErrorStatus { get; private set; }
    public string? ErrorMsg { get; private set; }
    private readonly IConnectionFactory _connectionFactory;
    private bool _disposed = false;

    public DataTableExecute(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<DataTable> ExecuteAsync(string storedProcedureName, Hashtable parameters)
    {
        ErrorStatus = false;
        ErrorMsg = null;

        var resultTable = new DataTable();

        await using (DbConnection connection = _connectionFactory.CreateConnection())
        await using (DbCommand command = connection.CreateCommand())
        {
            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 180;

            foreach (DictionaryEntry entry in parameters)
            {
                var dbParameter = command.CreateParameter();
                dbParameter.ParameterName = entry.Key.ToString();
                dbParameter.Value = entry.Value ?? DBNull.Value;
                command.Parameters.Add(dbParameter);
            }

            try
            {
                await connection.OpenAsync();

                await using (var reader = await command.ExecuteReaderAsync())
                {
                    resultTable.Load(reader);
                }

                ErrorStatus = true;
            }
            catch (DbException ex)
            {
                ErrorStatus = false;
                ErrorMsg = $"DB Error: {ex.Message}";
                Console.WriteLine(ErrorMsg);
            }
            catch (Exception ex)
            {
                ErrorStatus = false;
                ErrorMsg = $"General Error: {ex.Message}";
                Console.WriteLine(ErrorMsg);
            }
        }

        return resultTable;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;
        }
    }
}