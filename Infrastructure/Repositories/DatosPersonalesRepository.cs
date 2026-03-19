using System.Collections;
using System.Data;
using Domain.Commands;
using Domain.Entities;
using Domain.Ports;
using Infrastructure.Handlers;

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

    public async Task<DatosPersonales?> ExistsByCedula(string cedula)
    {
        return await ErrorHandler.HandleRepositoryErrorAsync(async () =>
        {
            _params.Clear();
            _params.Add("@Cedula", cedula);

            var datosPersonales = new DatosPersonales();

            _dt = await _dbCon.ExecuteAsync(nameof(EDatosPersonalesCommand.SPDatosPersonalesExistsCommand), _params);
            if (_dt.Rows.Count == 0) return datosPersonales;
            for (var i = 0; i < _dt.Rows.Count; i++)
            {
                datosPersonales = MapDataRowToDatosPersonales(_dt.Rows[i]);
            }
            return datosPersonales;
        }, "ExisteDatosPersonalesPorCedula");
    }

    public async Task<int> AddAsync(DatosPersonales datosPersonales)
    {
        return await ErrorHandler.HandleRepositoryErrorAsync(async () =>
        {
            _params.Clear();
            _params.Add("@UsuarioId", datosPersonales.UsuarioId);
            _params.Add("@Cedula", datosPersonales.Cedula);
            _params.Add("@Nombres", datosPersonales.Nombres);
            _params.Add("@Apellidos", datosPersonales.Apellidos);
            _params.Add("@Sexo", datosPersonales.Sexo);

            _dt = await _dbCon.ExecuteAsync(nameof(EDatosPersonalesCommand.SPDatosPersonalesAddCommand), _params);
            if (_dt.Rows.Count == 0) return 0;
            return Convert.ToInt32(_dt.Rows[0]["Id"]);
        }, "AgregarDatosPersonales");
    }

    private static DatosPersonales MapDataRowToDatosPersonales(DataRow row)
    {
        return new DatosPersonales
        {
            Id = Convert.ToInt32(row["Id"]),
            UsuarioId = Convert.ToInt32(row["UsuarioId"])
        };
    }
}