
namespace Domain.Exceptions
{
    [Serializable]
    public class UnableToCreateUserException : Exception
    {
        public UnableToCreateUserException()
        {
        }

        public UnableToCreateUserException(string? message) : base(message)
        {
        }

        public UnableToCreateUserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}