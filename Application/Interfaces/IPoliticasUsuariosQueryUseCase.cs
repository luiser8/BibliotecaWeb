using Domain.Entities;

namespace Application.Interfaces;

public interface IPoliticasUsuariosQueryUseCase
{
    Task<List<PoliticasUsuario>> GetPoliticasAsync(int rolId, string tipos);
}