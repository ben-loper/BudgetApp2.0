using AutoMapper;
using BackEnd.DTOs.BudgetDtos;
using BackEnd.DTOs.FamilyDtos;
using BackEnd.Utilities;
using Domain.Exceptions;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    public class TransactionController : BaseController<TransactionController>
    {
        private readonly ITransactionService _transactionService;
        private readonly IFamilyService _familyService;

        public TransactionController(ILogger<TransactionController> logger, IMapper mapper, IAuthService authService, ITransactionService transactionService, IFamilyService familyService) 
            : base(logger, mapper, authService)
        {
            _transactionService = transactionService;
            _familyService = familyService;
        }

        //TODO: Add error handling
        [HttpPost()]
        public async Task<ActionResult<FamilyDto>> AddTransactionToCategory(TransactionDto transaction)
        {
            if (transaction == null) return BadRequest();

            var family = await _familyService.GetFamilyByUserIdAsync(User.GetUserId());

            if (family == null) return BadRequest();

            var transactionToSave = _mapper.Map<Transaction>(transaction);

            family = await _transactionService.AddTransactionToBudgetCategory(family.Id, transaction.BudgetCategoryId, transactionToSave);

            var response = await MapFamilyToDto(family);

            return Ok(response);
        }

        //TODO: Add error handling
        [HttpPut()]
        public async Task<ActionResult<FamilyDto>> UpdateTransaction(TransactionDto transaction)
        {
            if (transaction == null) return BadRequest();

            var family = await _familyService.GetFamilyByUserIdAsync(User.GetUserId());

            if (family == null) return BadRequest();

            var transactionToSave = _mapper.Map<Transaction>(transaction);

            family = await _transactionService.UpdateTransaction(family.Id, transaction.BudgetCategoryId, transactionToSave);

            var response = await MapFamilyToDto(family);

            return Ok(response);
        }

        //TODO: Add error handling
        [HttpDelete("{budgetCategoryId}/{transactionId}")]
        public async Task<ActionResult<FamilyDto>> DeleteTransaction(string budgetCategoryId, string transactionId)
        {
            var family = await _familyService.GetFamilyByUserIdAsync(User.GetUserId());

            if (family == null) return BadRequest();

            family = await _transactionService.DeleteTransaction(family.Id, budgetCategoryId, transactionId);

            var response = await MapFamilyToDto(family);

            return Ok(response);
        }
    }
}
