USE [Biblioteca]
GO
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
    UPDATE dbo.Usuarios SET Contrasena = @NewContrasena
       WHERE Id = @Id;
END