SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SPUsuarioAuthCommand]
	@Correo VARCHAR(155) = NULL,
	@Contrasena VARCHAR(255) = NULL
AS
SET NOCOUNT ON;
BEGIN
	BEGIN TRY
		BEGIN
            SELECT 
                u.Id AS UsuarioId,
                dp.Cedula,
                u.Correo,
                u.Contrasena,
                dp.Nombres,
                dp.Apellidos,
                r.Id AS RolId,
                r.Nombre AS Rol,
                e.Nombre AS Extension,
                c.Nombre AS Carrera
            FROM Usuarios u
            INNER JOIN Extensiones e ON u.ExtensionId = e.Id
            INNER JOIN Roles r ON u.RolId = r.Id
            INNER JOIN DatosPersonales dp ON u.Id = dp.UsuarioId
            INNER JOIN DatosAcademicos da ON u.Id = da.UsuarioId
            INNER JOIN Carreras c ON da.CarreraId = c.Id
            WHERE u.Correo = @Correo
		END
	END TRY
		BEGIN CATCH
			SELECT ERROR_MESSAGE() AS ERROR,
				ERROR_NUMBER() AS ERROR_NRO
		END CATCH;
END
GO
