namespace Application.DTOs.Usuarios.Responses;

/// <summary>
/// Resultado genérico de una operación que puede tener éxito o error
/// </summary>
public class OperationResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorCode { get; set; }
    public Dictionary<string, string>? ValidationErrors { get; set; }

    public static OperationResult<T> Ok(T data)
    {
        return new OperationResult<T>
        {
            Success = true,
            Data = data
        };
    }

    public static OperationResult<T> Error(string errorMessage, string? errorCode = null)
    {
        return new OperationResult<T>
        {
            Success = false,
            ErrorMessage = errorMessage,
            ErrorCode = errorCode
        };
    }

    public static OperationResult<T> ValidationError(Dictionary<string, string> validationErrors)
    {
        return new OperationResult<T>
        {
            Success = false,
            ValidationErrors = validationErrors,
            ErrorMessage = "Errores de validación"
        };
    }
}

/// <summary>
/// Resultado simple sin datos (para operaciones que solo retornan éxito/error)
/// </summary>
public class OperationResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorCode { get; set; }
    public Dictionary<string, string>? ValidationErrors { get; set; }

    public static OperationResult Ok()
    {
        return new OperationResult
        {
            Success = true
        };
    }

    public static OperationResult Error(string errorMessage, string? errorCode = null)
    {
        return new OperationResult
        {
            Success = false,
            ErrorMessage = errorMessage,
            ErrorCode = errorCode
        };
    }

    public static OperationResult ValidationError(Dictionary<string, string> validationErrors)
    {
        return new OperationResult
        {
            Success = false,
            ValidationErrors = validationErrors,
            ErrorMessage = "Errores de validación"
        };
    }
}

