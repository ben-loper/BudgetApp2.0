
namespace Domain.Exceptions
{
    [Serializable]
    public class TransactionDoesNotExistException : Exception
    {
        public TransactionDoesNotExistException()
        {
        }

        public TransactionDoesNotExistException(string? message) : base(message)
        {
        }

        public TransactionDoesNotExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}