using System.Collections;
using System.Data;
using Domain.Commands;
using Domain.Entities;
using Domain.Ports;

namespace Infrastructure.Repositories;

public class CarreraRepository : ICarreraRepository
{
    private readonly IDataTableExecute _dbCon;
    private DataTable? _dt;
    private readonly Hashtable _params;

    public CarreraRepository(IDataTableExecute dataTableExecute)
    {
        _dt = new DataTable();
        _dbCon = dataTableExecute ?? throw new ArgumentNullException(nameof(dataTableExecute));
        _params = [];
    }

    public Task<Carrera> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Carrera>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<List<Carrera>> GetAllWithExtensionAsync(int id)
    {
        _params.Clear();
        _params.Add("@IdExtension", id);

        var listCarreras = new List<Carrera>();

        _dt = await _dbCon.ExecuteAsync(nameof(ECarreraCommand.SPCarreraAllWithExtensionCommand), _params);
        if (_dt.Rows.Count == 0) return listCarreras;
        for (var i = 0; i < _dt.Rows.Count; i++)
        {
            listCarreras.Add(MapDataRowToCarrera(_dt.Rows[i]));
        }
        return listCarreras;
    }
    
    private static Carrera MapDataRowToCarrera(DataRow row)
    {
        return new Carrera
        {
            Id = Convert.ToInt32(row["Id"]),
            Nombre = row["Nombre"].ToString(),
            Activo = Convert.ToInt32(row["Activo"]),
            FechaCreado = DateTime.Parse(row["FechaCreado"].ToString() ?? string.Empty)
        };
    }
}