-- Ver asignaciones por rol
SELECT 
    r.Nombre AS Rol,
    p.Id AS PoliticaId,
    p.Nombre AS Politica,
    p.Ruta
FROM Roles r
JOIN RolPoliticas rp ON r.Id = rp.RolId
JOIN Politicas p ON rp.PoliticaId = p.Id
ORDER BY r.Nombre, p.Id;

-- Conteo por rol
SELECT 
    r.Nombre AS Rol,
    COUNT(rp.PoliticaId) AS TotalPoliticas
FROM Roles r
LEFT JOIN RolPoliticas rp ON r.Id = rp.RolId
GROUP BY r.Nombre
ORDER BY TotalPoliticas DESC;