--EXTENSIONES
INSERT INTO Extensiones (Nombre, Descripcion, Estado, Ciudad, Direccion) VALUES
('Sede principal Barcelona', 'PSM Barcelona', 'Anzoátegui', 'Barcelona', 'Dirección no especificada'),
('Extensión Valencia', 'PSM Valencia', 'Carabobo', 'Valencia', 'Dirección no especificada'),
('Extensión Cabimas', 'PSM Cabimas', 'Zulia', 'Cabimas', 'Dirección no especificada'),
('Extensión Maracaibo', 'PSM Maracaibo', 'Zulia', 'Maracaibo', 'Dirección no especificada'),
('Extensión Caracas', 'PSM Caracas', 'Distrito Capital', 'Caracas', 'Dirección no especificada'),
('Extensión Mérida', 'PSM Mérida', 'Mérida', 'Mérida', 'Dirección no especificada'),
('Extensión San Cristóbal', 'PSM San Cristóbal', 'Táchira', 'San Cristóbal', 'Dirección no especificada'),
('Extensión Barinas', 'PSM Barinas', 'Barinas', 'Barinas', 'Dirección no especificada'),
('Extensión Maracay', 'PSM Maracay', 'Aragua', 'Maracay', 'Dirección no especificada'),
('Extensión Porlamar', 'PSM Porlamar', 'Nueva Esparta', 'Porlamar', 'Dirección no especificada'),
('Extensión Puerto Ordaz', 'PSM Puerto Ordaz', 'Bolívar', 'Puerto Ordaz', 'Dirección no especificada'),
('Extensión Maturín', 'PSM Maturín', 'Monagas', 'Maturín', 'Dirección no especificada'),
('Extensión Ciudad Ojeda', 'PSM Ciudad Ojeda', 'Zulia', 'Ciudad Ojeda', 'Dirección no especificada');

--CARRERAS
INSERT INTO Carreras (Nombre) VALUES
('Arquitectura'),
('Ingeniería Civil'),
('Ingeniería Eléctrica'),
('Ingeniería Electrónica'),
('Ingeniería Industrial'),
('Ingeniería en Mantenimiento Mecánico'),
('Ingeniería de Sistemas'),
('Ingeniería de Diseño Industrial'),
('Ingeniería Química'),
('Ingeniería de Petróleo'),
('Ingeniería Agronómica'),
('Ingeniería en Telecomunicaciones');

--EXTENSION CARRERAS
-- Arquitectura (disponible en todas las extensiones)
INSERT INTO ExtensionCarreras (ExtensionId, CarreraId) VALUES
(1, 1), (2, 1), (3, 1), (4, 1), (5, 1), (6, 1), (7, 1), (8, 1), (9, 1), (10, 1), (11, 1), (12, 1), (13, 1);

-- Ingeniería Civil (disponible en todas las extensiones)
INSERT INTO ExtensionCarreras (ExtensionId, CarreraId) VALUES
(1, 2), (2, 2), (3, 2), (4, 2), (5, 2), (6, 2), (7, 2), (8, 2), (9, 2), (10, 2), (11, 2), (12, 2), (13, 2);

-- Ingeniería Eléctrica (disponible en todas las extensiones)
INSERT INTO ExtensionCarreras (ExtensionId, CarreraId) VALUES
(1, 3), (2, 3), (3, 3), (4, 3), (5, 3), (6, 3), (7, 3), (8, 3), (9, 3), (10, 3), (11, 3), (12, 3), (13, 3);

-- Ingeniería Electrónica (disponible en todas las extensiones)
INSERT INTO ExtensionCarreras (ExtensionId, CarreraId) VALUES
(1, 4), (2, 4), (3, 4), (4, 4), (5, 4), (6, 4), (7, 4), (8, 4), (9, 4), (10, 4), (11, 4), (12, 4), (13, 4);

-- Ingeniería Industrial (disponible en todas las extensiones)
INSERT INTO ExtensionCarreras (ExtensionId, CarreraId) VALUES
(1, 5), (2, 5), (3, 5), (4, 5), (5, 5), (6, 5), (7, 5), (8, 5), (9, 5), (10, 5), (11, 5), (12, 5), (13, 5);

-- Ingeniería en Mantenimiento Mecánico (disponible en todas las extensiones)
INSERT INTO ExtensionCarreras (ExtensionId, CarreraId) VALUES
(1, 6), (2, 6), (3, 6), (4, 6), (5, 6), (6, 6), (7, 6), (8, 6), (9, 6), (10, 6), (11, 6), (12, 6), (13, 6);

-- Ingeniería de Sistemas (disponible en todas las extensiones)
INSERT INTO ExtensionCarreras (ExtensionId, CarreraId) VALUES
(1, 7), (2, 7), (3, 7), (4, 7), (5, 7), (6, 7), (7, 7), (8, 7), (9, 7), (10, 7), (11, 7), (12, 7), (13, 7);

-- Ingeniería de Diseño Industrial (disponible solo en 6 extensiones)
INSERT INTO ExtensionCarreras (ExtensionId, CarreraId) VALUES
(1, 8), (4, 8), (7, 8), (2, 8), (9, 8), (5, 8);

-- Ingeniería Química (disponible solo en 4 extensiones)
INSERT INTO ExtensionCarreras (ExtensionId, CarreraId) VALUES
(4, 9), (3, 9), (13, 9), (10, 9);

-- Ingeniería de Petróleo (disponible solo en 3 extensiones)
INSERT INTO ExtensionCarreras (ExtensionId, CarreraId) VALUES
(4, 10), (3, 10), (13, 10);

-- Ingeniería Agronómica (disponible solo en 1 extensión)
INSERT INTO ExtensionCarreras (ExtensionId, CarreraId) VALUES
(6, 11);

-- Ingeniería en Telecomunicaciones (disponible solo en 1 extensión)
INSERT INTO ExtensionCarreras (ExtensionId, CarreraId) VALUES
(5, 12);

--ROLES
INSERT INTO Roles (Nombre) VALUES
('Administrador'),
('Directivo'),
('Bibliotecario'),
('Estudiante'),
('Profesor');

-- POLÍTICAS CON RUTAS
INSERT INTO Politicas (Nombre, Ruta) VALUES
-- Módulo Usuarios
('UsuariosVer', 'Usuarios/Index'),
('UsuariosCrear', 'Usuarios/Crear'),
('UsuariosEditar', 'Usuarios/Editar'),
('UsuariosEliminar', 'Usuarios/Eliminar'),
('UsuariosConsultar', 'Usuarios/Consultar'),
('UsuariosAsignarRoles', 'Usuarios/AsignarRoles'),

-- Módulo Libros
('LibrosCrear', 'Libros/Crear'),
('LibrosEditar', 'Libros/Editar'),
('LibrosEliminar', 'Libros/Eliminar'),
('LibrosConsultar', 'Libros/Consultar'),
('LibrosPrestar', 'Libros/Prestar'),
('LibrosDevolver', 'Libros/Devolver'),

-- Módulo Préstamos
('PrestamosCrear', 'Prestamos/Crear'),
('PrestamosEditar', 'Prestamos/Editar'),
('PrestamosCancelar', 'Prestamos/Cancelar'),
('PrestamosConsultar', 'Prestamos/Consultar'),
('PrestamosReportes', 'Prestamos/Reportes'),

-- Módulo Extensiones
('ExtensionesCrear', 'Extensiones/Crear'),
('ExtensionesEditar', 'Extensiones/Editar'),
('ExtensionesEliminar', 'Extensiones/Eliminar'),
('ExtensionesConsultar', 'Extensiones/Consultar'),

-- Módulo Carreras
('CarrerasCrear', 'Carreras/Crear'),
('CarrerasEditar', 'Carreras/Editar'),
('CarrerasEliminar', 'Carreras/Eliminar'),
('CarrerasConsultar', 'Carreras/Consultar'),

-- Módulo Seguridad
('RolesCrear', '/Roles/Crear'),
('RolesEditar', 'Roles/Editar'),
('RolesEliminar', 'Roles/Eliminar'),
('RolesConsultar', 'Roles/Consultar'),
('RolesAsignar', 'Roles/Asignar'),

-- Módulo Politicas
('PoliticasCrear', 'Prestamos/Crear'),
('PoliticasEditar', 'Prestamos/Editar'),
('PoliticasConsultar', 'Prestamos/Consultar'),
('PoliticasEliminar', 'Prestamos/Eliminar'),

-- Módulo Reportes
('ReportesGenerar', 'Reportes/Generar'),
('ReportesConsultar', 'Reportes/Consultar'),

-- Módulo Auditoria
('AuditoriaConsultar', 'Auditoria/Consultar'),
('AuditoriaUsuario', 'Auditoria/Usuario/Id'),

-- Módulo Morosidad
('MorosidadConsultar', 'Morosidad/Consultar'),
('MorosidadUsuario', 'Morosidad/Usuario/Id'),

-- Módulo Perfil
('PerfilConsultar', 'Perfil/Consultar'),
('PerfilEditar', 'Perfil/Editar');

--ROL POLITICAS
-- 1. Obtener IDs de roles
DECLARE @AdminId INT = (SELECT Id FROM Roles WHERE Nombre = 'Administrador');
DECLARE @DirectivoId INT = (SELECT Id FROM Roles WHERE Nombre = 'Directivo');
DECLARE @BibliotecarioId INT = (SELECT Id FROM Roles WHERE Nombre = 'Bibliotecario');
DECLARE @ProfesorId INT = (SELECT Id FROM Roles WHERE Nombre = 'Profesor');
DECLARE @EstudianteId INT = (SELECT Id FROM Roles WHERE Nombre = 'Estudiante');

-- 2. Obtener IDs de políticas (ASEGÚRATE DE QUE EXISTAN)
DECLARE @LibrosConsultar INT = (SELECT Id FROM Politicas WHERE Nombre = 'LibrosConsultar');
DECLARE @LibrosPrestar INT = (SELECT Id FROM Politicas WHERE Nombre = 'LibrosPrestar');
DECLARE @PrestamosCrear INT = (SELECT Id FROM Politicas WHERE Nombre = 'PrestamosCrear');
DECLARE @PrestamosEditar INT = (SELECT Id FROM Politicas WHERE Nombre = 'PrestamosEditar');
DECLARE @PrestamosCancelar INT = (SELECT Id FROM Politicas WHERE Nombre = 'PrestamosCancelar');
DECLARE @PrestamosConsultar INT = (SELECT Id FROM Politicas WHERE Nombre = 'PrestamosConsultar');
DECLARE @PrestamosReportes INT = (SELECT Id FROM Politicas WHERE Nombre = 'PrestamosReportes');
DECLARE @PerfilConsultar INT = (SELECT Id FROM Politicas WHERE Nombre = 'PerfilConsultar');
DECLARE @PerfilEditar INT = (SELECT Id FROM Politicas WHERE Nombre = 'PerfilEditar');

-- 3. Limpiar asignaciones anteriores (opcional)
DELETE FROM RolPoliticas;

-- 4. ADMINISTRADOR: TODAS LAS POLÍTICAS (en orden)
INSERT INTO RolPoliticas (RolId, PoliticaId)
SELECT @AdminId, Id FROM Politicas ORDER BY Id;

-- 5. DIRECTIVO: Todo menos auditoria, seguridad y roles
INSERT INTO RolPoliticas (RolId, PoliticaId)
SELECT @DirectivoId, Id FROM Politicas 
WHERE Nombre NOT LIKE '%Auditoria%' 
  AND Nombre NOT LIKE 'Roles%'
  AND Ruta NOT LIKE 'Seguridad%'
ORDER BY Id;

-- 6. BIBLIOTECARIO: Todo menos auditoria, seguridad, roles, políticas, extensiones, carreras, usuarios
INSERT INTO RolPoliticas (RolId, PoliticaId)
SELECT @BibliotecarioId, Id FROM Politicas 
WHERE Nombre NOT LIKE '%Auditoria%'
  AND Nombre NOT LIKE 'Roles%'
  AND Ruta NOT LIKE 'Seguridad%'
  AND Nombre NOT LIKE '%Extensiones%'
  AND Nombre NOT LIKE '%Carreras%'
  AND Nombre NOT LIKE 'Usuarios%'
ORDER BY Id;

-- 7. PROFESOR: Solo políticas específicas
INSERT INTO RolPoliticas (RolId, PoliticaId) VALUES
(@ProfesorId, @LibrosConsultar),
(@ProfesorId, @LibrosPrestar),
(@ProfesorId, @PrestamosCrear),
(@ProfesorId, @PrestamosEditar),
(@ProfesorId, @PrestamosCancelar),
(@ProfesorId, @PrestamosConsultar),
(@ProfesorId, @PrestamosReportes),
(@ProfesorId, @PerfilConsultar),
(@ProfesorId, @PerfilEditar);

-- 8. ESTUDIANTE: Igual que profesor
INSERT INTO RolPoliticas (RolId, PoliticaId) VALUES
(@EstudianteId, @LibrosConsultar),
(@EstudianteId, @LibrosPrestar),
(@EstudianteId, @PrestamosCrear),
(@EstudianteId, @PrestamosEditar),
(@EstudianteId, @PrestamosCancelar),
(@EstudianteId, @PrestamosConsultar),
(@EstudianteId, @PerfilConsultar),
(@EstudianteId, @PerfilEditar);
