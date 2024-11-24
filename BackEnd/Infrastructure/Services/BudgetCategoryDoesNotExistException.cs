
namespace Infrastructure.Services
{
    [Serializable]
    internal class BudgetCategoryDoesNotExistException : Exception
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