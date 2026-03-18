SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SPUsuarioRecuperacionAddCodigoCommand]
    @UsuarioId INT = NULL,
    @Codigo VARCHAR(155) = NULL
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @SCOPEIDENTITY INT;
    DECLARE @Email VARCHAR(255);
    
    -- Obtener el email del usuario
    SELECT @Email = Correo 
    FROM dbo.Usuarios 
    WHERE Id = @UsuarioId;
    
    -- Insertar en la tabla de recuperación
    INSERT INTO dbo.UsuariosRecuperacion(UsuarioId, Codigo)
    VALUES(@UsuarioId, @Codigo);
    
    SET @SCOPEIDENTITY = SCOPE_IDENTITY();
    
    -- Retornar el resultado
    SELECT 
        @SCOPEIDENTITY AS Id,
        @Email AS Email,
        NULL AS ErrorMessage,
        NULL AS ErrorCode
END
GO
