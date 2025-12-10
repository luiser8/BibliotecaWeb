using Application.DTOs.Usuarios.Request;
using Application.DTOs.Usuarios.Responses;
using Application.Interfaces;
using Domain.Entities;
using Domain.Ports;
using Infrastructure.Exceptions;

namespace Application.UseCases.Usuarios;

public class UsuarioCommandUseCase(IUsuarioRepository usuarioRepository, IDatosPersonalesRepository datosPersonalesRepository, IDatosAcademicosRepository datosAcademicosRepository) : IUsuarioCommandUseCase
{
    public async Task<OperationResult<int>> ExecuteInsertAsync(UsuarioCreateDto usuario)
    {
        try
        {
            var saveUsuario = await usuarioRepository.AddAsync(new Usuario
            {
                ExtensionId =  usuario.ExtensionId,
                RolId =  usuario.RolId,
                Correo =  usuario.Correo,
                Contrasena = usuario.Contrasena
            });
            
            await datosPersonalesRepository.AddAsync(new DatosPersonales
            {
                UsuarioId = saveUsuario,
                Cedula = usuario.DatosPersonales.Cedula,
                Nombres = usuario.DatosPersonales.Nombres,
                Apellidos = usuario.DatosPersonales.Apellidos,
                FechaNacimiento = usuario.DatosPersonales.FechaNacimiento,
                Sexo = usuario.DatosPersonales.Sexo,
            });
            
            if(usuario.DatosPersonales != null && usuario.RolId == 4)
            {
                await datosAcademicosRepository.AddAsync(new DatosAcademicos
                {
                    UsuarioId = saveUsuario,
                    CarreraId = usuario.DatosAcademicos.CarreraId,
                    TipoIngreso =  usuario.DatosAcademicos.TipoIngreso,
                });
            }
            
            return OperationResult<int>.Ok(saveUsuario);
        }
        catch (DuplicateRecordException ex)
        {
            var validationErrors = new Dictionary<string, string>
            {
                { ex.FieldName ?? "General", ex.Message }
            };
            return OperationResult<int>.ValidationError(validationErrors);
        }
        catch (ForeignKeyViolationException ex)
        {
            return OperationResult<int>.Error(ex.Message, ex.ErrorCode);
        }
        catch (RepositoryException ex)
        {
            return OperationResult<int>.Error(ex.Message, ex.ErrorCode);
        }
        catch (ArgumentException ex)
        {
            return OperationResult<int>.Error(ex.Message);
        }
        catch (Exception ex)
        {
            return OperationResult<int>.Error($"Error al crear usuario: {ex.Message}");
        }
    }
}