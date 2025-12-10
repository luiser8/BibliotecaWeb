SELECT 
    r.Nombre AS Rol,
    p.Nombre AS Politica,
    p.Id AS PoliticaId
FROM Roles r
INNER JOIN RolPoliticas rp ON r.Id = rp.RolId
INNER JOIN Politicas p ON rp.PoliticaId = p.Id
ORDER BY r.Nombre, p.Nombre;

SELECT 
    r.Id AS RolId,
    r.Nombre AS Rol,
    (
        SELECT p.Nombre
        FROM RolPoliticas rp
        INNER JOIN Politicas p ON rp.PoliticaId = p.Id
        WHERE rp.RolId = r.Id
        FOR JSON PATH
    ) AS Politicas
FROM Roles r
WHERE r.Activo = 1;