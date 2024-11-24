
namespace Domain.Exceptions
{
    [Serializable]
    public class BudgetCategoryDoesNotExistException : Exception
    {
        public BudgetCategoryDoesNotExistException()
        {
        }

        public BudgetCategoryDoesNotExistException(string? message) : base(message)
        {
        }

        public BudgetCategoryDoesNotExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}