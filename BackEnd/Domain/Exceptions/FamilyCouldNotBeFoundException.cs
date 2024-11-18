
namespace Domain.Exceptions
{
    [Serializable]
    public class FamilyCouldNotBeFoundException : Exception
    {
        public FamilyCouldNotBeFoundException()
        {
        }

        public FamilyCouldNotBeFoundException(string? message) : base(message)
        {
        }

        public FamilyCouldNotBeFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}