
using AutoMapper;
using BackEnd.DTOs.BudgetDtos;
using BackEnd.DTOs.FamilyDtos;
using BackEnd.Utilities;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    public class BudgetController : BaseController<BudgetController>
    {
        private readonly IBudgetService _budgetService;
        private readonly IFamilyService _familyService;

        public BudgetController(ILogger<BudgetController> logger, IMapper mapper, IAuthService authService, IBudgetService budgetService, IFamilyService familyService) : base(logger, mapper, authService)
        {
            _budgetService = budgetService;
            _familyService = familyService;
        }

        // TODO: Need more graceful handling of errors
        [HttpPost]
        public async Task<ActionResult<FamilyDto>> CreateBudgetCategory(BudgetCategoryDto category)
        {
            if (category == null) return BadRequest();

            var family = await _familyService.GetFamilyByUserIdAsync(User.GetUserId());

            if (family == null || family.Id == null)
            {
                _logger.LogError("User is not assigned to a family or no budget exists for family");
                return BadRequest();
            }

            var budgetCategory = _mapper.Map<BudgetCategory>(category);

            try
            {
                family = await _budgetService.CreateCategoryForBudgetAsync(family.Id, budgetCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error while attempting to save category for budget - {ex}", ex);
                return BadRequest();
            }
            
            var response = _mapper.Map<FamilyDto>(family);
            
            return Ok(response);
        }

        [HttpPost("MonthlyBill")]
        public async Task<ActionResult<FamilyDto>> AddMonthlyBill(MonthlyBillDto request)
        {
            if (request == null) return BadRequest();

            var family = await _familyService.GetFamilyByUserIdAsync(User.GetUserId());

            if (family == null || family.Id == null)
            {
                _logger.LogError("User is not assigned to a family or no budget exists for family");
                return BadRequest();
            }

            var monthlyBill = _mapper.Map<MonthlyBill>(request);

            try
            {
                family = await _budgetService.CreateMonthlyBillAsync(family.Id, monthlyBill);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error while attempting to save category for budget - {ex}", ex);
                return BadRequest();
            }

            var response = _mapper.Map<FamilyDto>(family);

            return Ok(response);
        }
    }
}
