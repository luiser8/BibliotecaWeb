using System.Collections;
using System.Data;
using Domain.Ports;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Configurations;

public class DataTableExecute(IConnectionFactory connectionFactory) : IDataTableExecute, IDisposable
{
    public bool ErrorStatus { get; private set; }
    private string? ErrorMsg { get; set; }
    private readonly IConnectionFactory _connectionFactory = connectionFactory
                                                             ?? throw new ArgumentNullException(nameof(connectionFactory));
    private bool _disposed = false;

    public async Task<DataTable> ExecuteAsync(string storedProcedureName, Hashtable? parameters)
    {
        ErrorStatus = false;
        ErrorMsg = null;

        var resultTable = new DataTable();

        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand(storedProcedureName, connection);
        command.CommandType = CommandType.StoredProcedure;
        command.CommandTimeout = 180;

        if (parameters != null)
        {
            foreach (DictionaryEntry entry in parameters)
            {
                command.Parameters.AddWithValue(
                    entry.Key.ToString(),
                    entry.Value ?? DBNull.Value
                );
            }
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
        catch (SqlException ex)
        {
            ErrorStatus = false;
            ErrorMsg = $"SQL Error: {ex.Message}";
            Console.WriteLine(ErrorMsg);
        }
        catch (Exception ex)
        {
            ErrorStatus = false;
            ErrorMsg = $"General Error: {ex.Message}";
            Console.WriteLine(ErrorMsg);
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