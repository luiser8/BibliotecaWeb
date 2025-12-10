using System.Collections;
using System.Data;
using Domain.Commands;
using Domain.Entities;
using Domain.Ports;

namespace Infrastructure.Repositories;

public class DatosAcademicosRepository : IDatosAcademicosRepository
{
    private readonly IDataTableExecute _dbCon;
    private DataTable? _dt;
    private readonly Hashtable _params;
    
    public DatosAcademicosRepository(IDataTableExecute dataTableExecute)
    {
        _dt = new DataTable();
        _dbCon = dataTableExecute ?? throw new ArgumentNullException(nameof(dataTableExecute));
        _params = [];
    }
    
    public async Task<int> AddAsync(DatosAcademicos datosAcademicos)
    {
        try
        {
            _params.Clear();
            _params.Add("@UsuarioId", datosAcademicos.UsuarioId);
            _params.Add("@CarreraId", datosAcademicos.CarreraId);
            _params.Add("@TipoIngreso", datosAcademicos.TipoIngreso);

            _dt = await _dbCon.ExecuteAsync(nameof(EDatosAcademicosCommand.SPDatosAcademicosAddCommand), _params);
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