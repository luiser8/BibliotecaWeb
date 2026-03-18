using Domain.Entities;

namespace Domain.Ports;

public interface IDatosPersonalesRepository
{
    Task<DatosPersonales?> ExistsByCedula(string cedula);
    Task<int> AddAsync(DatosPersonales datosPersonales);
}