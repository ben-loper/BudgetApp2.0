
namespace Domain.Exceptions
{
    [Serializable]
    public class UserAlreadyInFamilyException : Exception
    {
        public UserAlreadyInFamilyException()
        {
        }

        public UserAlreadyInFamilyException(string? message) : base(message)
        {
        }

        public UserAlreadyInFamilyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}