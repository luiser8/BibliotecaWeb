-- Insertar el usuario administrador
INSERT INTO Usuarios (ExtensionId, RolId, Correo, Contrasena) 
VALUES (1, (SELECT TOP 1 Id FROM Roles WHERE Nombre = 'Administrador'), 'administrador@psm.edu.ve', 'dp6G+V4XfwVMMCCBJPqLCw==:a9IE8oPJRxdSapJJ64cfKxIGS1jlL7NJogbpesHzhgs=');

-- Insertar el usuario directivo
INSERT INTO Usuarios (ExtensionId, RolId, Correo, Contrasena) 
VALUES (1, (SELECT TOP 1 Id FROM Roles WHERE Nombre = 'Directivo'), 'directivo@psm.edu.ve', 'dp6G+V4XfwVMMCCBJPqLCw==:a9IE8oPJRxdSapJJ64cfKxIGS1jlL7NJogbpesHzhgs=');

-- Insertar el usuario biblitecario
INSERT INTO Usuarios (ExtensionId, RolId, Correo, Contrasena) 
VALUES (1, (SELECT TOP 1 Id FROM Roles WHERE Nombre = 'Bibliotecario'), 'bibliotecario@psm.edu.ve', 'dp6G+V4XfwVMMCCBJPqLCw==:a9IE8oPJRxdSapJJ64cfKxIGS1jlL7NJogbpesHzhgs=');

-- Insertar el usuario profesor
INSERT INTO Usuarios (ExtensionId, RolId, Correo, Contrasena) 
VALUES (1, (SELECT TOP 1 Id FROM Roles WHERE Nombre = 'Profesor'), 'profesor@psm.edu.ve', 'dp6G+V4XfwVMMCCBJPqLCw==:a9IE8oPJRxdSapJJ64cfKxIGS1jlL7NJogbpesHzhgs=');

-- Insertar el usuario estudiante nacional
INSERT INTO Usuarios (ExtensionId, RolId, Correo, Contrasena) 
VALUES (1, (SELECT TOP 1 Id FROM Roles WHERE Nombre = 'Estudiante'), 'estudiante_nacional@psm.edu.ve', 'dp6G+V4XfwVMMCCBJPqLCw==:a9IE8oPJRxdSapJJ64cfKxIGS1jlL7NJogbpesHzhgs=');

-- Insertar el usuario estudiante internacional
INSERT INTO Usuarios (ExtensionId, RolId, Correo, Contrasena) 
VALUES (1, (SELECT TOP 1 Id FROM Roles WHERE Nombre = 'Estudiante'), 'estudiante_internacional@psm.edu.ve', 'dp6G+V4XfwVMMCCBJPqLCw==:a9IE8oPJRxdSapJJ64cfKxIGS1jlL7NJogbpesHzhgs=');

-- Obtener el ID del usuario insertado (asumamos que es 1)

-- Insertar datos personales del administrador
INSERT INTO DatosPersonales (UsuarioId, Cedula, Nombres, Apellidos, Sexo)
VALUES ((SELECT TOP 1 Id FROM Usuarios WHERE Correo = 'administrador@psm.edu.ve'), '32432425', 'Administrador', '', 'Masculino');

-- Insertar datos personales del directivo
INSERT INTO DatosPersonales (UsuarioId, Cedula, Nombres, Apellidos, Sexo)
VALUES ((SELECT TOP 1 Id FROM Usuarios WHERE Correo = 'directivo@psm.edu.ve'), '12432421', 'Directivo', '', 'Masculino');

-- Insertar datos personales del bibliotecario
INSERT INTO DatosPersonales (UsuarioId, Cedula, Nombres, Apellidos, Sexo)
VALUES ((SELECT TOP 1 Id FROM Usuarios WHERE Correo = 'bibliotecario@psm.edu.ve'), '22432421', 'Bibliotecario', '', 'Masculino');

-- Insertar datos personales del bibliotecario
INSERT INTO DatosPersonales (UsuarioId, Cedula, Nombres, Apellidos, Sexo)
VALUES ((SELECT TOP 1 Id FROM Usuarios WHERE Correo = 'profesor@psm.edu.ve'), '42432421', 'Profesor', '', 'Masculino');

-- Insertar datos personales del estudiante nacional
INSERT INTO DatosPersonales (UsuarioId, Cedula, Nombres, Apellidos, Sexo)
VALUES ((SELECT TOP 1 Id FROM Usuarios WHERE Correo = 'estudiante_nacional@psm.edu.ve'), '52432421', 'Estudiante', '', 'Masculino');

-- Insertar datos personales del estudiante internacional
INSERT INTO DatosPersonales (UsuarioId, Cedula, Nombres, Apellidos, Sexo)
VALUES ((SELECT TOP 1 Id FROM Usuarios WHERE Correo = 'estudiante_internacional@psm.edu.ve'), '62432421', 'Estudiante', '', 'Masculino');

-- Insertar datos académicos del estudiante nacional
INSERT INTO DatosAcademicos (UsuarioId, CarreraId, TipoIngreso)
VALUES ((SELECT TOP 1 Id FROM Usuarios WHERE Correo = 'estudiante_nacional@psm.edu.ve'), 
(SELECT TOP 1 Id FROM Carreras WHERE Nombre = 'Arquitectura'), 'Nacional');

-- Insertar datos académicos del estudiante internacional
INSERT INTO DatosAcademicos (UsuarioId, CarreraId, TipoIngreso)
VALUES ((SELECT TOP 1 Id FROM Usuarios WHERE Correo = 'estudiante_internacional@psm.edu.ve'), 
(SELECT TOP 1 Id FROM Carreras WHERE Nombre = 'Ingeniería Civil'), 'Internacional');

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