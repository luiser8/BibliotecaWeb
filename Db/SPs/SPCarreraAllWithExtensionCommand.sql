SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SPCarreraAllWithExtensionCommand]
	@IdExtension INT = NULL
AS
SET NOCOUNT ON;
BEGIN
	BEGIN TRY
		BEGIN
			IF @IdExtension != 0
                SELECT 
                    c.Id,
                    c.Nombre,
                    c.Activo,
                    ec.FechaCreado
                FROM ExtensionCarreras ec
                INNER JOIN Carreras c ON ec.CarreraId = c.Id
                WHERE ec.ExtensionId = @IdExtension AND ec.Activo = 1
                ORDER BY c.Id;
		END
	END TRY
		BEGIN CATCH
			SELECT ERROR_MESSAGE() AS ERROR,
				ERROR_NUMBER() AS ERROR_NRO
		END CATCH;
END
GO
