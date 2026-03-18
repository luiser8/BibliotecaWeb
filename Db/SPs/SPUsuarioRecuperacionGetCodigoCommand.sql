SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SPUsuarioRecuperacionGetCodigoCommand]
	@Codigo VARCHAR(155) = NULL
AS
SET NOCOUNT ON;
BEGIN
    SELECT Id, UsuarioId, Codigo, Activo, FechaCreado FROM dbo.UsuariosRecuperacion WHERE Codigo = @Codigo
END
GO
