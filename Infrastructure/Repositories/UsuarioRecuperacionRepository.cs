using System.Collections;
using System.Data;
using Domain.Commands;
using Domain.Entities;
using Domain.Ports;
using Infrastructure.Handlers;

namespace Infrastructure.Repositories;

public class UsuarioRecuperacionRepository : IUsuarioRecuperacionRepository
{
    private readonly IDataTableExecute _dbCon;
    private DataTable? _dt;
    private readonly Hashtable _params;

    public UsuarioRecuperacionRepository(IDataTableExecute dataTableExecute)
    {
        _dt = new DataTable();
        _dbCon = dataTableExecute ?? throw new ArgumentNullException(nameof(dataTableExecute));
        _params = [];
    }

    public async Task<string?> RecoveryAsync(int usuarioId, string codigo)
    {
        return await ErrorHandler.HandleRepositoryErrorAsync(async () =>
        {
            _params.Clear();
            _params.Add("@UsuarioId", usuarioId);
            _params.Add("@Codigo", codigo);

            _dt = await _dbCon.ExecuteAsync(nameof(EUsuariosRecuperacionCommand.SPUsuarioRecuperacionAddCodigoCommand), _params);
            return _dt.Rows.Count == 0 ? string.Empty : Convert.ToString(_dt.Rows[0]["Email"]);
        }, "GuardarCodigoRecuperacion");
    }

    public async Task<UsuarioRecuperacion> VerificarCodigoUsuario(string codigo)
    {
        return await ErrorHandler.HandleRepositoryErrorAsync(async () =>
        {
            _params.Clear();
            _params.Add("@Codigo", codigo);

            var usuarioRecuperacion = new UsuarioRecuperacion();

            _dt = await _dbCon.ExecuteAsync(nameof(EUsuariosRecuperacionCommand.SPUsuarioRecuperacionGetCodigoCommand), _params);
            if (_dt.Rows.Count == 0) return usuarioRecuperacion;
            for (var i = 0; i < _dt.Rows.Count; i++)
            {
                usuarioRecuperacion = MapDataRowToUsuarioRecuperacion(_dt.Rows[i]);
            }
            return usuarioRecuperacion;
        }, "ObtenerUsuarioRecuperacion");
    }

    public async Task<bool> VencimientoCodigo(string codigo)
    {
        return await ErrorHandler.HandleRepositoryErrorAsync(async () =>
        {
            _params.Clear();
            _params.Add("@Codigo", codigo);

            _dt = await _dbCon.ExecuteAsync(nameof(EUsuariosRecuperacionCommand.SPUsuarioRecuperacionEditCodigoCommand), _params);
            return _dt.Rows.Count != 0;
        }, "GuardarVencimientoCodigo");
    }

    private static UsuarioRecuperacion MapDataRowToUsuarioRecuperacion(DataRow row)
    {
        return new UsuarioRecuperacion
        {
            Id = Convert.ToInt32(row["Id"]),
            UsuarioId = Convert.ToInt32(row["UsuarioId"]),
            Cedula = row["Cedula"].ToString(),
            Codigo = row["Codigo"].ToString(),
            Activo = row["Activo"] != DBNull.Value && Convert.ToByte(row["Activo"]) == 1,
            FechaCreado = DateTime.Parse(row["FechaCreado"].ToString() ?? string.Empty)
        };
    }
}