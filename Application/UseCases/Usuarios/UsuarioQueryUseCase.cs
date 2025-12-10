using Application.DTOs.Usuarios.Responses;
using Application.Interfaces;
using Domain.Entities;
using Domain.Ports;
using Infrastructure.Exceptions;

namespace Application.UseCases.Usuarios;

public class UsuarioQueryUseCase(IUsuarioRepository usuarioRepository, IDatosPersonalesRepository datosPersonalesRepository, IDatosAcademicosRepository datosAcademicosRepository) : IUsuarioQueryUseCase
{

}