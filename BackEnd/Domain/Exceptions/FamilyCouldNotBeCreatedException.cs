
namespace Domain.Exceptions
{
    [Serializable]
    public class FamilyCouldNotBeCreatedException : Exception
    {
        public FamilyCouldNotBeCreatedException()
        {
        }

        public FamilyCouldNotBeCreatedException(string? message) : base(message)
        {
        }

        public FamilyCouldNotBeCreatedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}