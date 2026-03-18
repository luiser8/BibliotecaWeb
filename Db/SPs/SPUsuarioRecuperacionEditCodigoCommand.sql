SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SPUsuarioRecuperacionEditCodigoCommand]
    @Codigo VARCHAR(255) = NULL
AS
SET NOCOUNT ON;
BEGIN
    UPDATE dbo.UsuariosRecuperacion SET Activo = 0 WHERE Codigo = @Codigo;
END
GO
