using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ITransactionService
    {
        public Task<Family> AddTransactionToBudgetCategory(string familyId, string categoryId, Transaction transaction);
        public Task<Family> UpdateTransaction(string familyId, string categoryId, Transaction transaction);
        public Task<Family> DeleteTransaction(string familyId, string categoryId, string transactionId);
    }
}
