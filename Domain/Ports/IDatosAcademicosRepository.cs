using Domain.Entities;

namespace Domain.Ports;

public interface IDatosAcademicosRepository
{
    Task<int> AddAsync(DatosAcademicos datosAcademicos);
}