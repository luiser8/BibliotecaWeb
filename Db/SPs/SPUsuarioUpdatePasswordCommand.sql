SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SPUsuarioUpdatePasswordCommand]
    @Id INT = NULL,
    @NewContrasena VARCHAR(255) = NULL
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @SCOPEIDENTITY INT;
        UPDATE dbo.Usuarios SET Contrasena = @NewContrasena WHERE Id = @Id;
        SET @SCOPEIDENTITY = SCOPE_IDENTITY();

        SELECT 
        @SCOPEIDENTITY AS Id,
        NULL AS ErrorMessage,
        NULL AS ErrorCode
END
GO
