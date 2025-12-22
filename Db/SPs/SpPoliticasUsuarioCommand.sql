SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SpPoliticasUsuarioCommand]
@RolId INT = NULL
AS
SET NOCOUNT ON;
BEGIN
	BEGIN TRY
		BEGIN
            SELECT p.Id AS PoliticaId, p.Nombre, p.Ruta
            FROM RolPoliticas rp
            INNER JOIN Politicas p ON rp.PoliticaId = p.Id
            WHERE rp.RolId = @RolId
            ORDER BY p.Id;
		END
	END TRY
		BEGIN CATCH
			SELECT ERROR_MESSAGE() AS ERROR,
				ERROR_NUMBER() AS ERROR_NRO
		END CATCH;
END
GO
