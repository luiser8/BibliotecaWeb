SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SPExtensionAllCarreraCommand]

AS
SET NOCOUNT ON;
BEGIN
	BEGIN TRY
		BEGIN
            SELECT 
                e.Id AS ExtensionId,
                e.Nombre,
                (
                        SELECT 
                            e.Id,
                            e.Nombre,
                            e.Descripcion,
                            e.Direccion,
                            e.Estado,
                            e.Ciudad,
                            e.Defecto,
                            e.Activo,
                            e.FechaCreado
                        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                    ) AS Extension,
                e.Ciudad,
                e.Estado,
                e.Direccion,
                e.Activo,
                (
                    SELECT 
                        c.Id,
                        c.Nombre,
                        c.Activo,
                        ec.FechaCreado
                    FROM ExtensionCarreras ec
                    INNER JOIN Carreras c ON ec.CarreraId = c.Id
                    WHERE ec.ExtensionId = e.Id AND ec.Activo = 1
                    FOR JSON PATH
                ) AS Carreras
            FROM Extensiones e
            WHERE e.Activo = 1
            ORDER BY e.Id;
		END
	END TRY
		BEGIN CATCH
			SELECT ERROR_MESSAGE() AS ERROR,
				ERROR_NUMBER() AS ERROR_NRO
		END CATCH;
END
GO
