-- Insertar el usuario estudiante
INSERT INTO Usuarios (ExtensionId, RolId, Correo, Contrasena) 
VALUES (1, 4, 'estudiante.ejemplo@psm.edu.ve', 'hashed_password_123');

-- Obtener el ID del usuario insertado (asumamos que es 1)

-- Insertar datos personales del estudiante
INSERT INTO DatosPersonales (UsuarioId, Cedula, Nombres, Apellidos, FechaNacimiento, Sexo)
VALUES (3, '32432424', 'María Fernanda', 'González Pérez', '2000-05-15', 'Femenino');

-- Insertar datos académicos del estudiante (cursando Arquitectura)
INSERT INTO DatosAcademicos (UsuarioId, CarreraId, TipoIngreso)
VALUES (3, 1, 'Nacional');

--query 1
SELECT 
    -- Información del Usuario
    u.Id AS UsuarioId,
    u.Correo,
    u.Activo AS UsuarioActivo,
    u.FechaCreado AS FechaRegistro,
    
    -- Información de la Extensión
    e.Id AS ExtensionId,
    e.Nombre AS Extension,
    e.Ciudad,
    e.Estado,
    
    -- Información del Tipo de Usuario y Rol
    r.Id AS RolId,
    r.Nombre AS Rol,
    
    -- Datos Personales
    dp.Nombres,
    dp.Apellidos,
    dp.FechaNacimiento,
    dp.Sexo,
    
    -- Datos Académicos
    da.TipoIngreso,
    c.Nombre AS Carrera,
    
    -- Políticas del Rol (en formato JSON)
    (
        SELECT p.Nombre AS Politica
        FROM RolPoliticas rp
        INNER JOIN Politicas p ON rp.PoliticaId = p.Id
        WHERE rp.RolId = r.Id
        FOR JSON PATH
    ) AS PoliticasAcceso
    
FROM Usuarios u
INNER JOIN Extensiones e ON u.ExtensionId = e.Id
INNER JOIN Roles r ON u.RolId = r.Id
INNER JOIN DatosPersonales dp ON u.Id = dp.UsuarioId
INNER JOIN DatosAcademicos da ON u.Id = da.UsuarioId
INNER JOIN Carreras c ON da.CarreraId = c.Id
WHERE u.Id = 1;
--query 2
            SELECT 
                u.Id AS UsuarioId,
                dp.Cedula,
                u.Correo,
                dp.Nombres,
                dp.Apellidos,
                r.Id AS RolId,
                r.Nombre AS Rol,
                e.Nombre AS Extension,
                c.Nombre AS Carrera,
                REPLACE((
                        SELECT p.Nombre, p.Ruta
                        FROM RolPoliticas rp
                        INNER JOIN Politicas p ON rp.PoliticaId = p.Id
                        WHERE rp.RolId = r.Id
                        FOR JSON PATH
                    ), '\/', '/') AS Politicas
            FROM Usuarios u
            INNER JOIN Extensiones e ON u.ExtensionId = e.Id
            INNER JOIN Roles r ON u.RolId = r.Id
            INNER JOIN DatosPersonales dp ON u.Id = dp.UsuarioId
            INNER JOIN DatosAcademicos da ON u.Id = da.UsuarioId
            INNER JOIN Carreras c ON da.CarreraId = c.Id
WHERE u.Correo = 'leduardo.rondon@gmail.com' 
    AND u.Contrasena = '2C5WilqLRQJWTNO056nGUA==:iLxzWmTmfV2lZ+gByEaI68B7NKnP54uYgTNt3lgGURk='
    --u.Id = 1;