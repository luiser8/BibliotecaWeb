using Infrastructure.Exceptions;

namespace Presentation.Services;

/// <summary>
/// Servicio para manejar excepciones y convertirlas en mensajes de error amigables
/// </summary>
public class ExceptionHandlerService
{
    /// <summary>
    /// Obtiene un mensaje de error amigable basado en el tipo de excepción
    /// </summary>
    public string GetErrorMessage(Exception exception)
    {
        // Manejar excepciones específicas primero
        if (exception is DuplicateRecordException duplicateEx)
            return GetDuplicateRecordMessage(duplicateEx);

        if (exception is RecordNotFoundException notFoundEx)
            return GetRecordNotFoundMessage(notFoundEx);

        if (exception is ForeignKeyViolationException fkEx)
            return GetForeignKeyViolationMessage(fkEx);

        if (exception is RepositoryException repoEx)
            return GetRepositoryExceptionMessage(repoEx);

        // ArgumentNullException hereda de ArgumentException, así que verificamos primero
        if (exception is ArgumentNullException || exception is ArgumentException)
            return exception.Message;

        return "Ocurrió un error inesperado. Por favor, intenta nuevamente o contacta al administrador.";
    }

    /// <summary>
    /// Obtiene el código de error basado en el tipo de excepción
    /// </summary>
    public string? GetErrorCode(Exception exception)
    {
        return exception switch
        {
            DuplicateRecordException ex => ex.ErrorCode,
            RecordNotFoundException ex => ex.ErrorCode,
            ForeignKeyViolationException ex => ex.ErrorCode,
            RepositoryException ex => ex.ErrorCode,
            _ => null
        };
    }

    /// <summary>
    /// Obtiene el nombre del campo relacionado con el error (si aplica)
    /// </summary>
    public string? GetFieldName(Exception exception)
    {
        return exception switch
        {
            DuplicateRecordException ex => ex.FieldName,
            _ => null
        };
    }

    /// <summary>
    /// Agrega el error al ModelState de forma apropiada
    /// </summary>
    public void AddErrorToModelState(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState, Exception exception)
    {
        var fieldName = GetFieldName(exception);
        var errorMessage = GetErrorMessage(exception);

        if (!string.IsNullOrEmpty(fieldName))
        {
            modelState.AddModelError(fieldName, errorMessage);
        }
        else
        {
            modelState.AddModelError("", errorMessage);
        }
    }

    private string GetDuplicateRecordMessage(DuplicateRecordException ex)
    {
        return ex.FieldName switch
        {
            "Correo" => $"El correo electrónico '{ex.FieldValue}' ya está registrado. Por favor, utiliza otro correo o intenta iniciar sesión.",
            "ISBN" => $"El ISBN '{ex.FieldValue}' ya está registrado en el sistema.",
            "Nombre" => $"Ya existe un registro con el nombre '{ex.FieldValue}'. Por favor, utiliza otro nombre.",
            _ => ex.Message
        };
    }

    private string GetRecordNotFoundMessage(RecordNotFoundException ex)
    {
        return ex.EntityName switch
        {
            "Usuario" => "Correo electrónico o contraseña incorrectos. Por favor, verifica tus credenciales.",
            _ => ex.Message
        };
    }

    private string GetForeignKeyViolationMessage(ForeignKeyViolationException ex)
    {
        return "No se puede completar la operación debido a datos relacionados inválidos. Por favor, verifica la información.";
    }

    private string GetRepositoryExceptionMessage(RepositoryException ex)
    {
        return ex.ErrorCode switch
        {
            "DB_TIMEOUT" => "El servidor está tardando demasiado en responder. Por favor, intenta nuevamente en unos momentos.",
            "DB_UNAVAILABLE" => "El servicio no está disponible temporalmente. Por favor, intenta más tarde.",
            "DB_AUTH_FAILED" => "Error de autenticación con la base de datos. Contacte al administrador.",
            "ADD_FAILED" => "No se pudo crear el registro. Por favor, verifica los datos e intenta nuevamente.",
            "UPDATE_FAILED" => "No se pudo actualizar el registro. Por favor, verifica los datos e intenta nuevamente.",
            "DELETE_FAILED" => "No se pudo eliminar el registro. Por favor, intenta nuevamente.",
            "AUTH_FAILED" => ex.Message ?? "Credenciales incorrectas. Por favor intenta nuevamente.",
            "DEADLOCK" => "Se detectó un conflicto en la base de datos. Por favor, intenta nuevamente.",
            _ => ex.Message ?? "Ocurrió un error en la operación. Por favor, intenta nuevamente."
        };
    }
}

