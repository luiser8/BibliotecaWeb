using System.Collections;
using System.Data;
using Domain.Commands;
using Domain.Entities;
using Domain.Ports;
using Infrastructure.Handlers;

namespace Infrastructure.Repositories;

public class PoliticasUsuarioRepository : IPoliticasUsuarioRepository
{
    private readonly IDataTableExecute _dbCon;
    private DataTable? _dt;
    private readonly Hashtable _params;

    public PoliticasUsuarioRepository(
        IDataTableExecute dataTableExecute)
    {
        _dt = new DataTable();
        _dbCon = dataTableExecute ?? throw new ArgumentNullException(nameof(dataTableExecute));
        _params = [];
    }

    public async Task<List<PoliticasUsuario>> GetPoliticasAsync(int rolId, string tipos)
    {
        return await ErrorHandler.HandleRepositoryErrorAsync(async () =>
        {
            var result = new List<PoliticasUsuario>();

            _params.Clear();
            _params.Add("@RolId", rolId);
            _params.Add("@Tipos", tipos);
            _dt = await _dbCon.ExecuteAsync(nameof(EPoliticasUsuarioCommand.SpPoliticasUsuarioCommand), _params);

            foreach (DataRow row in _dt.Rows)
            {
                var itemArray = row.ItemArray;
                var columnNames = _dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
                
                var index = Array.IndexOf(columnNames, "PoliticaId");
                var tipoIndex = Array.IndexOf(columnNames, "Tipo");
                var nombreIndex = Array.IndexOf(columnNames, "Nombre");
                var rutaIndex = Array.IndexOf(columnNames, "Ruta");

                var politicas = new PoliticasUsuario
                {
                    PoliticaId = Convert.ToInt32(itemArray[index]),
                    Tipo = itemArray[tipoIndex]?.ToString() ?? string.Empty,
                    Nombre = itemArray[nombreIndex]?.ToString() ?? string.Empty,
                    Ruta = itemArray[rutaIndex]?.ToString() ?? string.Empty,
                };
            
                result.Add(politicas);
            }

            return result;
        }, "ObtenerPoliticasUsuario");
    }
}