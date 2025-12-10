using System.Collections;
using System.Data;
using System.Text.Json;
using Domain.Commands;
using Domain.Entities;
using Domain.Ports;

namespace Infrastructure.Repositories;

public class ExtensionRepository : IExtensionRepository
{
    private readonly IDataTableExecute _dbCon;
    private DataTable? _dt;
    private readonly Hashtable _params;

    public ExtensionRepository(IDataTableExecute dataTableExecute)
    {
        _dt = new DataTable();
        _dbCon = dataTableExecute ?? throw new ArgumentNullException(nameof(dataTableExecute));
        _params = [];
    }
    
    public Task<Extension> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Extension>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<List<ExtensionCarrera>> GetAllWithCarrerasAsync()
    {
        var result = new List<ExtensionCarrera>();

        _dt = await _dbCon.ExecuteAsync(nameof(EExtensionCommand.SPExtensionAllCarreraCommand), _params);

        foreach (DataRow row in _dt.Rows)
        {
            var itemArray = row.ItemArray;
            var columnNames = _dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
            
            var extensionIdIndex = Array.IndexOf(columnNames, "ExtensionId");
            var nombreIndex = Array.IndexOf(columnNames, "Nombre");
            var ciudadIndex = Array.IndexOf(columnNames, "Ciudad");
            var estadoIndex = Array.IndexOf(columnNames, "Estado");
            var direccionIndex = Array.IndexOf(columnNames, "Direccion");
            var extensionIndex = Array.IndexOf(columnNames, "Extension");
            var carrerasIndex = Array.IndexOf(columnNames, "Carreras");

            var extension = new ExtensionCarrera
            {
                ExtensionId = Convert.ToInt32(itemArray[extensionIdIndex]),
                Nombre = itemArray[nombreIndex]?.ToString() ?? string.Empty,
                Ciudad = itemArray[ciudadIndex]?.ToString() ?? string.Empty,
                Estado = itemArray[estadoIndex]?.ToString() ?? string.Empty,
                Direccion = itemArray[direccionIndex]?.ToString() ?? string.Empty,
                Extension = null,
                Carreras = [],
            };
        
            var extensionJson = itemArray[extensionIndex]?.ToString();
            if (!string.IsNullOrEmpty(extensionJson))
            {
                try
                {
                    extension.Extension = JsonSerializer.Deserialize<Extension>(extensionJson);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error deserializando Extension: {ex.Message}");
                    Console.WriteLine($"JSON: {extensionJson}");
                }
            }
        
            var carrerasJson = itemArray[carrerasIndex]?.ToString();
            if (!string.IsNullOrEmpty(carrerasJson))
            {
                try
                {
                    var carreras = JsonSerializer.Deserialize<List<Carrera>>(carrerasJson);
                    if (carreras != null)
                    {
                        extension.Carreras?.AddRange(carreras);
                        Console.WriteLine($"✅ Se agregaron {carreras.Count} carreras");
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"❌ Error deserializando Carreras: {ex.Message}");
                    Console.WriteLine($"JSON: {carrerasJson}");
                }
            }
        
            result.Add(extension);
        }

        return result;
    }
}