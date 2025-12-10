using Infrastructure.Exceptions;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Handlers;

    public static class ErrorHandler
    {
        public static T HandleRepositoryError<T>(Func<T> operation, string operationName = null)
        {
            try
            {
                return operation();
            }
            catch (RepositoryException)
            {
                throw; // Re-lanzar excepciones específicas del repositorio
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
        
        public static async Task<T> HandleRepositoryErrorAsync<T>(Func<Task<T>> operation, string operationName = null)
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
        
        public static Exception HandleSqlException(SqlException sqlEx, string operationName = null)
        {
            string errorContext = operationName ?? "operación de base de datos";
            
            switch (sqlEx.Number)
            {
                // Violación de UNIQUE constraint
                case 2601:
                case 2627:
                    return HandleUniqueConstraintViolation(sqlEx);
                
                // Violación de FOREIGN KEY constraint
                case 547:
                    return HandleForeignKeyViolation(sqlEx);
                
                // Timeout
                case -2:
                    return new RepositoryException("DB_TIMEOUT", 
                        $"Timeout en la {errorContext}. Por favor, intente nuevamente.");
                
                // No se puede abrir la base de datos
                case 4060:
                    return new RepositoryException("DB_UNAVAILABLE", 
                        "La base de datos no está disponible. Contacte al administrador.");
                
                // Error de login
                case 18456:
                    return new RepositoryException("DB_AUTH_FAILED", 
                        "Error de autenticación con la base de datos.");
                
                // Deadlock
                case 1205:
                    return new RepositoryException("DEADLOCK", 
                        $"Deadlock detectado en {errorContext}. Intente nuevamente.");
                
                default:
                    return new RepositoryException($"DB_ERROR_{sqlEx.Number}", 
                        $"Error de base de datos en {errorContext}: {sqlEx.Message}")
                    {
                        SqlErrorNumber = sqlEx.Number
                    };
            }
        }
        
        private static RepositoryException HandleUniqueConstraintViolation(SqlException sqlEx)
        {
            // Extraer el nombre del campo del mensaje de error
            string errorMessage = sqlEx.Message;
            
            if (errorMessage.Contains("Correo") || errorMessage.Contains("UQ_Usuarios_Correo"))
            {
                return new DuplicateRecordException("Correo", "valor duplicado");
            }
            else if (errorMessage.Contains("ISBN") || errorMessage.Contains("UQ_Libros_ISBN"))
            {
                return new DuplicateRecordException("ISBN", "valor duplicado");
            }
            else if (errorMessage.Contains("Nombre") && errorMessage.Contains("UQ_"))
            {
                // Intentar extraer el valor del nombre duplicado
                var match = System.Text.RegularExpressions.Regex.Match(
                    errorMessage, @"\('([^']*)'\)");
                string duplicateValue = match.Success ? match.Groups[1].Value : "valor duplicado";
                
                return new DuplicateRecordException("Nombre", duplicateValue);
            }
            
            // Mensaje genérico para otras violaciones UNIQUE
            return new DuplicateRecordException("campo único", "valor duplicado");
        }
        
        private static RepositoryException HandleForeignKeyViolation(SqlException sqlEx)
        {
            string errorMessage = sqlEx.Message;
            
            // Intentar extraer información de la constraint
            if (errorMessage.Contains("FK_"))
            {
                var match = System.Text.RegularExpressions.Regex.Match(
                    errorMessage, @"constraint ""([^""]*)""");
                
                if (match.Success)
                {
                    string constraintName = match.Groups[1].Value;
                    string tableName = constraintName.Replace("FK_", "").Split('_')[0];
                    
                    return new ForeignKeyViolationException(constraintName, tableName);
                }
            }
            
            return new RepositoryException("FOREIGN_KEY_VIOLATION", 
                "No se puede completar la operación porque hay registros relacionados.");
        }
        
        private static RepositoryException HandleGenericException(Exception ex, string operationName)
        {
            string errorContext = operationName ?? "operación";
            
            return new RepositoryException("UNEXPECTED_ERROR", 
                $"Error inesperado en {errorContext}: {ex.Message}")
            {
                SqlErrorNumber = null
            };
        }
        
        // Método para validaciones comunes
        public static void ValidateNotNull(object entity, string entityName)
        {
            if (entity == null)
                throw new ArgumentNullException(entityName, $"{entityName} no puede ser nulo");
        }
        
        public static void ValidateId(object id, string idName)
        {
            if (id == null || (id is int intId && intId <= 0))
                throw new ArgumentException($"{idName} no es válido", idName);
        }
    }
