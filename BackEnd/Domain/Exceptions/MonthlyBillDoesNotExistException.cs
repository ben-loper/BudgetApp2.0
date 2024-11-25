
namespace Domain.Exceptions
{
    [Serializable]
    public class MonthlyBillDoesNotExistException : Exception
    {
        public MonthlyBillDoesNotExistException()
        {
        }

        public MonthlyBillDoesNotExistException(string? message) : base(message)
        {
        }

        public MonthlyBillDoesNotExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}