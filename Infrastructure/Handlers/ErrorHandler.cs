using Domain.Exceptions;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Handlers;

public static class ErrorHandler
{
    /// <summary>
    /// Ejecuta una operación de repositorio y traduce errores técnicos (SqlException, etc.)
    /// a excepciones de dominio más amigables (RepositoryException y derivadas).
    /// </summary>
    public static T HandleRepositoryError<T>(Func<T> operation, string? operationName = null)
    {
        try
        {
            return operation();
        }
        catch (RepositoryException)
        {
            // Re-lanzar excepciones específicas del repositorio tal cual
            throw;
        }
        catch (SqlException sqlEx)
        {
            throw HandleSqlException(sqlEx, operationName);
        }
        catch (Exception ex)
        {
            throw HandleGenericException(ex, operationName);
        }
    }

    /// <summary>
    /// Versión asíncrona de <see cref="HandleRepositoryError{T}"/>.
    /// </summary>
    public static async Task<T> HandleRepositoryErrorAsync<T>(Func<Task<T>> operation, string? operationName = null)
    {
        try
        {
            return await operation();
        }
        catch (RepositoryException)
        {
            throw;
        }
        catch (SqlException sqlEx)
        {
            throw HandleSqlException(sqlEx, operationName);
        }
        catch (Exception ex)
        {
            throw HandleGenericException(ex, operationName);
        }
    }

    /// <summary>
    /// Traduce un <see cref="SqlException"/> a una excepción de dominio más específica.
    /// </summary>
    public static Exception HandleSqlException(SqlException sqlEx, string? operationName = null)
    {
        var errorContext = operationName ?? "operación de base de datos";

        return sqlEx.Number switch
        {
            // Violación de UNIQUE constraint
            2601 or 2627 => HandleUniqueConstraintViolation(sqlEx),

            // Violación de FOREIGN KEY constraint
            547 => HandleForeignKeyViolation(sqlEx),

            // Timeout
            -2 => new RepositoryException("DB_TIMEOUT",
                $"Timeout en la {errorContext}. Por favor, intente nuevamente."),

            // No se puede abrir la base de datos
            4060 => new RepositoryException("DB_UNAVAILABLE",
                "La base de datos no está disponible. Contacte al administrador."),

            // Error de login
            18456 => new RepositoryException("DB_AUTH_FAILED",
                "Error de autenticación con la base de datos."),

            // Deadlock
            1205 => new RepositoryException("DEADLOCK",
                $"Deadlock detectado en {errorContext}. Intente nuevamente."),

            // Cualquier otro error de SQL
            _ => new RepositoryException($"DB_ERROR_{sqlEx.Number}",
                $"Error de base de datos en {errorContext}: {sqlEx.Message}")
            {
                SqlErrorNumber = sqlEx.Number
            }
        };
    }

    private static RepositoryException HandleUniqueConstraintViolation(SqlException sqlEx)
    {
        // Extraer el nombre del campo del mensaje de error
        var errorMessage = sqlEx.Message;

        if (errorMessage.Contains("Correo", StringComparison.OrdinalIgnoreCase) ||
            errorMessage.Contains("UQ_Usuarios_Correo", StringComparison.OrdinalIgnoreCase))
        {
            return new DuplicateRecordException("Correo", "valor duplicado");
        }

        if (errorMessage.Contains("ISBN", StringComparison.OrdinalIgnoreCase) ||
            errorMessage.Contains("UQ_Libros_ISBN", StringComparison.OrdinalIgnoreCase))
        {
            return new DuplicateRecordException("ISBN", "valor duplicado");
        }

        if (errorMessage.Contains("Nombre", StringComparison.OrdinalIgnoreCase) &&
            errorMessage.Contains("UQ_", StringComparison.OrdinalIgnoreCase))
        {
            // Intentar extraer el valor del nombre duplicado
            var match = System.Text.RegularExpressions.Regex.Match(
                errorMessage, @"\('([^']*)'\)");
            var duplicateValue = match.Success ? match.Groups[1].Value : "valor duplicado";

            return new DuplicateRecordException("Nombre", duplicateValue);
        }

        // Mensaje genérico para otras violaciones UNIQUE
        return new DuplicateRecordException("campo único", "valor duplicado");
    }

    private static RepositoryException HandleForeignKeyViolation(SqlException sqlEx)
    {
        var errorMessage = sqlEx.Message;

        // Intentar extraer información de la constraint
        if (errorMessage.Contains("FK_", StringComparison.OrdinalIgnoreCase))
        {
            var match = System.Text.RegularExpressions.Regex.Match(
                errorMessage, @"constraint ""([^""]*)""");

            if (match.Success)
            {
                var constraintName = match.Groups[1].Value;
                var tableName = constraintName.Replace("FK_", string.Empty).Split('_')[0];

                return new ForeignKeyViolationException(constraintName, tableName);
            }
        }

        return new RepositoryException("FOREIGN_KEY_VIOLATION",
            "No se puede completar la operación porque hay registros relacionados.");
    }

    private static RepositoryException HandleGenericException(Exception ex, string? operationName)
    {
        var errorContext = operationName ?? "operación";

        return new RepositoryException("UNEXPECTED_ERROR",
            $"Error inesperado en {errorContext}: {ex.Message}")
        {
            SqlErrorNumber = null
        };
    }

    // Método para validaciones comunes
    public static void ValidateNotNull(object? entity, string entityName)
    {
        if (entity is null)
            throw new ArgumentNullException(entityName, $"{entityName} no puede ser nulo");
    }

    public static void ValidateId(object? id, string idName)
    {
        if (id is null || (id is int intId && intId <= 0))
            throw new ArgumentException($"{idName} no es válido", idName);
    }
}
