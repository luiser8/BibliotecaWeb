SELECT 
    tu.Id,
    tu.Tipo,
    r.Nombre AS Rol,
    tu.Activo,
    tu.FechaCreado
FROM TiposUsuarios tu
INNER JOIN Roles r ON tu.RolId = r.Id
ORDER BY tu.Id;

SELECT 
    tu.Id AS TipoUsuarioId,
    tu.Tipo,
    r.Nombre AS Rol,
    (
        SELECT p.Nombre
        FROM RolPoliticas rp
        INNER JOIN Politicas p ON rp.PoliticaId = p.Id
        WHERE rp.RolId = r.Id
        FOR JSON PATH
    ) AS Politicas
FROM TiposUsuarios tu
INNER JOIN Roles r ON tu.RolId = r.Id
WHERE tu.Activo = 1;