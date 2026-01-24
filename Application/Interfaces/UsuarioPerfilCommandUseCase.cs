namespace Application.Interfaces
{
    public interface IUsuarioPerfilCommandUseCase
    {
        Task<bool> CambiarContrasenaAsync(int UsuarioId, string newPassword);
    }
}
