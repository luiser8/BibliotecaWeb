using Application.Interfaces;
using Domain.Entities;
using Domain.Ports;

namespace Application.UseCases.Politicas;

public class PoliticasUsuariosQueryUseCase : IPoliticasUsuariosQueryUseCase
{
    private readonly IPoliticasUsuarioRepository _politicasUsuarioRepository;
    
    public PoliticasUsuariosQueryUseCase(IPoliticasUsuarioRepository politicasUsuarioRepository)
    {
        _politicasUsuarioRepository = politicasUsuarioRepository;
    }
    
    public Task<List<PoliticasUsuario>> GetPoliticasAsync(int rolId, string tipos)
    {
        return _politicasUsuarioRepository.GetPoliticasAsync(rolId, tipos);
    }
}