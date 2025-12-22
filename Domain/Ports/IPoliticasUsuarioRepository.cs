using Domain.Entities;

namespace Domain.Ports;

public interface IPoliticasUsuarioRepository
{
    Task<List<PoliticasUsuario>> GetPoliticasAsync(int rolId);
}