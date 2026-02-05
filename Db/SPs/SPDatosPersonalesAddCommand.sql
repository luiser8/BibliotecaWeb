SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SPDatosPersonalesAddCommand]
	@UsuarioId INT = NULL,
	@Cedula VARCHAR(155) = NULL,
	@Nombres VARCHAR(255) = NULL,
    @Apellidos VARCHAR(255) = NULL,
    @Sexo VARCHAR(15) = NULL
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @SCOPEIDENTITY INT;
    
    BEGIN TRY
        INSERT INTO dbo.DatosPersonales(UsuarioId, Cedula, Nombres, Apellidos, Sexo)
			VALUES(@UsuarioId, @Cedula, @Nombres, @Apellidos, @Sexo);
        
        SET @SCOPEIDENTITY = SCOPE_IDENTITY();
        
        SELECT 
            @SCOPEIDENTITY AS Id,
            NULL AS ErrorMessage,
            NULL AS ErrorCode
    END TRY
	BEGIN CATCH
        DECLARE @ErrorNumber INT = ERROR_NUMBER();
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        
        -- Código 2601: Violación de índice único (UNIQUE constraint)
        -- Código 2627: Violación de constraint UNIQUE KEY
        IF @ErrorNumber = 2601 OR @ErrorNumber = 2627
        BEGIN
            SELECT 
                -1 AS Id,
                'Cedula ya está registrada en el sistema' AS ErrorMessage,
                @ErrorNumber AS ErrorCode
        END
        ELSE
        BEGIN
            SELECT 
                -1 AS Id,
                'Error inesperado: ' + @ErrorMessage AS ErrorMessage,
                @ErrorNumber AS ErrorCode
        END
    END CATCH;
END
GO
