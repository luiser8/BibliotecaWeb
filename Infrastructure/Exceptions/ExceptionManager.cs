namespace Infrastructure.Exceptions;

    // Excepción base para repositorios
    public class RepositoryException : Exception
    {
        public string ErrorCode { get; set; }
        public int? SqlErrorNumber { get; set; }
        
        public RepositoryException(string message) : base(message) { }
        public RepositoryException(string message, Exception inner) : base(message, inner) { }
        public RepositoryException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
    
    // Excepción para registros duplicados
    public class DuplicateRecordException : RepositoryException
    {
        public string FieldName { get; set; }
        public object FieldValue { get; set; }
        
        public DuplicateRecordException(string fieldName, object fieldValue) 
            : base("DUPLICATE_RECORD", $"Ya existe un registro con {fieldName} = '{fieldValue}'")
        {
            FieldName = fieldName;
            FieldValue = fieldValue;
        }
    }
    
    // Excepción para registro no encontrado
    public class RecordNotFoundException : RepositoryException
    {
        public string EntityName { get; set; }
        public object EntityId { get; set; }
        
        public RecordNotFoundException(string entityName, object entityId) 
            : base("RECORD_NOT_FOUND", $"{entityName} con ID {entityId} no encontrado")
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
    
    // Excepción para violación de FK
    public class ForeignKeyViolationException : RepositoryException
    {
        public string ConstraintName { get; set; }
        public string TableName { get; set; }
        
        public ForeignKeyViolationException(string constraintName, string tableName) 
            : base("FOREIGN_KEY_VIOLATION", $"No se puede eliminar/modificar porque hay registros relacionados en {tableName}")
        {
            ConstraintName = constraintName;
            TableName = tableName;
        }
    }
    
    // Excepción para operación no permitida
    public class OperationNotAllowedException : RepositoryException
    {
        public string Operation { get; set; }
        
        public OperationNotAllowedException(string operation, string reason) 
            : base("OPERATION_NOT_ALLOWED", $"Operación '{operation}' no permitida: {reason}")
        {
            Operation = operation;
        }
    }
