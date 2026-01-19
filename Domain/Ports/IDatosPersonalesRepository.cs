using Domain.Entities;

namespace Domain.Ports;

public interface IDatosPersonalesRepository
{
    Task<bool> ExistsByCedula(string cedula);
    Task<int> AddAsync(DatosPersonales datosPersonales);
}