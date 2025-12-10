SELECT 
    e.Id AS ExtensionId,
    e.Nombre AS Extension,
    e.Ciudad,
    e.Estado,
    e.Direccion,
    e.Activo AS ExtensionActiva,
    (
        SELECT 
            c.Id,
            c.Nombre AS Carrera,
            c.Activo AS CarreraActiva,
            ec.FechaCreado
        FROM ExtensionCarreras ec
        INNER JOIN Carreras c ON ec.CarreraId = c.Id
        WHERE ec.ExtensionId = e.Id AND ec.Activo = 1
        FOR JSON PATH
    ) AS Carreras
FROM Extensiones e
WHERE e.Activo = 1
ORDER BY e.Id;