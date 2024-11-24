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

            var response = await MapFamilyToDto(family);

            return Ok(response);
        }

        // TODO: Need more graceful handling of errors
        [HttpPost("BudgetCategory")]
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

            var response = await MapFamilyToDto(family);

            return Ok(response);
        }

        [HttpPut("BudgetCategory")]
        public async Task<ActionResult<FamilyDto>> UpdateMonthlyBill(UpdateBudgetCategoryRequest request)
        {
            if (request == null) return BadRequest();

            var family = await _familyService.GetFamilyByUserIdAsync(User.GetUserId());

            if (family == null || family.Id == null
                || family.Budget == null)
            {
                _logger.LogError("User is not assigned to a family or no budget exists for family");
                return BadRequest();
            }

            try
            {
                family = await _budgetService.UpdateCategoryAsync(family.Id, request.CategoryId, request.Name, request.Amount);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error while attempting to update category for budget - {ex}", ex);
                return BadRequest();
            }

            var response = await MapFamilyToDto(family);

            return Ok(response);
        }

        [HttpDelete("BudgetCategory/{categoryId}")]
        public async Task<ActionResult<FamilyDto>> DeleteCategory(string categoryId)
        {
            var family = await _familyService.GetFamilyByUserIdAsync(User.GetUserId());

            if (family == null || family.Id == null
                || family.Budget == null)
            {
                _logger.LogError("User is not assigned to a family or no budget exists for family");
                return BadRequest();
            }

            try
            {
                family = await _budgetService.DeleteCategoryAsync(family.Id, categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error while attempting to delete category for budget - {ex}", ex);
                return BadRequest();
            }

            var response = await MapFamilyToDto(family);

            return Ok(response);
        }
    }
}
