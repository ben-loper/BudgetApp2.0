using Domain.Exceptions;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;

using Transaction = Domain.Models.Transaction;

namespace Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IFamilyRepository _familyRepo;

        public TransactionService(IFamilyRepository familyRepository)
        {
            _familyRepo = familyRepository;
        }

        public async Task<Family> AddTransactionToBudgetCategory(string familyId, string categoryId, Transaction transaction)
        {
            var family = await _familyRepo.GetFamilyByIdAsync(familyId);

            if (family == null) throw new FamilyCouldNotBeFoundException();

            if (family.Budget == null) throw new BudgetDoesNotExistForFamilyException();

            var budgetCategory = family.Budget.BudgetCategories.Where(b => b.Id == categoryId).FirstOrDefault();

            if (budgetCategory == null) throw new BudgetDoesNotExistForFamilyException();

            budgetCategory.Transactions.Add(transaction);

            return await _familyRepo.UpdateFamilyAsync(family);
        }

        public async Task<Family> DeleteTransaction(string familyId, string categoryId, string transactionId)
        {
            var family = await _familyRepo.GetFamilyByIdAsync(familyId);

            if (family == null) throw new FamilyCouldNotBeFoundException();

            if (family.Budget == null) throw new BudgetDoesNotExistForFamilyException();

            var budgetCategory = family.Budget.BudgetCategories.Where(b => b.Id == categoryId).FirstOrDefault();

            if (budgetCategory == null) throw new BudgetDoesNotExistForFamilyException();

            var transactionToRemove = budgetCategory.Transactions.Where(t => t.Id == transactionId).FirstOrDefault();

            if (transactionToRemove == null) throw new TransactionDoesNotExistException();

            budgetCategory.Transactions.Remove(transactionToRemove);

            return await _familyRepo.UpdateFamilyAsync(family);
        }

        public async Task<Family> UpdateTransaction(string familyId, string categoryId, Transaction transaction)
        {
            var family = await _familyRepo.GetFamilyByIdAsync(familyId);

            if (family == null) throw new FamilyCouldNotBeFoundException();

            if (family.Budget == null) throw new BudgetDoesNotExistForFamilyException();

            var budgetCategory = family.Budget.BudgetCategories.Where(b => b.Id == categoryId).FirstOrDefault();

            if (budgetCategory == null) throw new BudgetDoesNotExistForFamilyException();

            var transactionToUpdate = budgetCategory.Transactions.Where(t => t.Id == transaction.Id).FirstOrDefault();

            if (transactionToUpdate == null) throw new TransactionDoesNotExistException();

            budgetCategory.Transactions.Remove(transactionToUpdate);

            budgetCategory.Transactions.Add(transaction);

            return await _familyRepo.UpdateFamilyAsync(family);
        }
    }
}
