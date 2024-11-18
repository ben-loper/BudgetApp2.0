
namespace Domain.Exceptions
{
    [Serializable]
    public class BudgetAlreadyExistsForFamilyException : Exception
    {
        public BudgetAlreadyExistsForFamilyException()
        {
        }

        public BudgetAlreadyExistsForFamilyException(string? message) : base(message)
        {
        }

        public BudgetAlreadyExistsForFamilyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}