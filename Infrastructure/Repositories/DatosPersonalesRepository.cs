using System.Collections;
using System.Data;
using Domain.Commands;
using Domain.Entities;
using Domain.Ports;

namespace Infrastructure.Repositories;

public class DatosPersonalesRepository : IDatosPersonalesRepository
{
    private readonly IDataTableExecute _dbCon;
    private DataTable? _dt;
    private readonly Hashtable _params;
    
    public DatosPersonalesRepository(IDataTableExecute dataTableExecute)
    {
        _dt = new DataTable();
        _dbCon = dataTableExecute ?? throw new ArgumentNullException(nameof(dataTableExecute));
        _params = [];
    }
    
    public async Task<int> AddAsync(DatosPersonales datosPersonales)
    {
        try
        {
            _params.Clear();
            _params.Add("@UsuarioId", datosPersonales.UsuarioId);
            _params.Add("@Cedula", datosPersonales.Cedula);
            _params.Add("@Nombres", datosPersonales.Nombres);
            _params.Add("@Apellidos", datosPersonales.Apellidos);
            _params.Add("@FechaNacimiento", datosPersonales.FechaNacimiento);
            _params.Add("@Sexo", datosPersonales.Sexo);

            _dt = await _dbCon.ExecuteAsync(nameof(EDatosPersonalesCommand.SPDatosPersonalesAddCommand), _params);
            if (_dt.Rows.Count == 0) return 0;
            return Convert.ToInt32(_dt.Rows[0]["Id"]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}