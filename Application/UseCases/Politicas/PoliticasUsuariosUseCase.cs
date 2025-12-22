using Application.Interfaces;
using Domain.Entities;
using Domain.Ports;

namespace Application.UseCases.Politicas;

public class PoliticasUsuariosUseCase : IPoliticasUsuarioUseCase
{
    private readonly IPoliticasUsuarioRepository _politicasUsuarioRepository;
    
    public PoliticasUsuariosUseCase(IPoliticasUsuarioRepository politicasUsuarioRepository)
    {
        _politicasUsuarioRepository = politicasUsuarioRepository;
    }
    
    public Task<List<PoliticasUsuario>> GetPoliticasAsync(int rolId)
    {
        return _politicasUsuarioRepository.GetPoliticasAsync(rolId);
    }
}