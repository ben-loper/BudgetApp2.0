
namespace Domain.Exceptions
{
    [Serializable]
    public class FamilyDoesNotExistException : Exception
    {
        public FamilyDoesNotExistException()
        {
        }

        public FamilyDoesNotExistException(string? message) : base(message)
        {
        }

        public FamilyDoesNotExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}