using Domain.Entities;
using Domain.Ports;

namespace Application.UseCases;

public class UsuarioUseCase(IUsuarioRepository usuarioRepository, IDatosPersonalesRepository datosPersonalesRepository, IDatosAcademicosRepository datosAcademicosRepository)
{
    public async Task<int> ExecuteInsertAsync(Usuario usuario)
    {
        var saveUsuario = await usuarioRepository.AddAsync(usuario);
        usuario.Id = saveUsuario;
        await datosPersonalesRepository.AddAsync(usuario.DatosPersonales);
        await datosAcademicosRepository.AddAsync(usuario.DatosAcademicos);
        return saveUsuario;
    }

    public async Task<Usuario> ExecuteLoginAsync(string email, string password)
    {
        return await usuarioRepository.AuthAsync(email, password);
    }
    
    public async Task<Usuario> ExecuteLogOutAsync(string email, string password)
    {
        return await usuarioRepository.AuthAsync(email, password);
    }
}