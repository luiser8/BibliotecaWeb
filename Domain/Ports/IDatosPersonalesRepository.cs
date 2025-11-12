using Domain.Entities;

namespace Domain.Ports;

public interface IDatosPersonalesRepository
{
    Task<int> AddAsync(DatosPersonales datosPersonales);
}