
namespace Domain.Exceptions
{
    [Serializable]
    public class UserDoesNotExistInFamily : Exception
    {
        public UserDoesNotExistInFamily()
        {
        }

        public UserDoesNotExistInFamily(string? message) : base(message)
        {
        }

        public UserDoesNotExistInFamily(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}