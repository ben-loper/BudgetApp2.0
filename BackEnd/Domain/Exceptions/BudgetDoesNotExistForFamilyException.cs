
namespace Domain.Exceptions
{
    [Serializable]
    public class BudgetDoesNotExistForFamilyException : Exception
    {
        public BudgetDoesNotExistForFamilyException()
        {
        }

        public BudgetDoesNotExistForFamilyException(string? message) : base(message)
        {
        }

        public BudgetDoesNotExistForFamilyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}