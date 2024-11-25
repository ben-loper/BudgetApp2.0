using AutoMapper;
using BackEnd.DTOs.FamilyDtos;
using BackEnd.Utilities;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BaseController<T>(ILogger<T> logger, IMapper mapper, IAuthService authService) : ControllerBase
    {
        protected readonly ILogger<T> _logger = logger;
        protected readonly IMapper _mapper = mapper;
        protected readonly IAuthService _authService = authService;

        //TODO: This really needs to be refactored into something more manageable
        // Adding transaction logic is really making this bloated
        internal async Task<FamilyDto> MapFamilyToDto(Family family)
        {
            var familyDto = _mapper.Map<FamilyDto>(family);

            //TODO: Abstract this to a service or method
            foreach (var userId in family.AdminUserIds)
            {
                var user = await _authService.GetUserAsync(userId);
                familyDto.AdminUsers.Add(user.GetUsername());

                if (familyDto.Budget != null) familyDto.Budget.PayThisMonth += user.TotalPayThisMonth();
            }

            foreach (var userId in family.MemberUserIds)
            {
                var user = await _authService.GetUserAsync(userId);
                familyDto.Members.Add(user.GetUsername());

                if (familyDto.Budget != null) familyDto.Budget.PayThisMonth += user.TotalPayThisMonth();
            }

            if (familyDto.Budget is not null)
            {
                var firstOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                DateTime lastDayOfMonth;

                if (firstOfTheMonth.Month == 12) lastDayOfMonth = new DateTime(DateTime.Now.Year + 1, 1, 1, 0, 0, 0).AddDays(-1);
                else lastDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1, 23, 59, 59).AddDays(-1);

                decimal transactionsTotal = 0;

                foreach (var category in familyDto.Budget.BudgetCategories)
                {
                    category.Transactions = category.Transactions.Where(t => t.TransactionDate >= firstOfTheMonth && t.TransactionDate <= lastDayOfMonth).ToList();

                    var transactionsThisMonth = category.Transactions.Sum(t => t.Amount);
                    category.AmountRemaining = category.Amount - transactionsThisMonth;

                    transactionsTotal += transactionsThisMonth;
                }

                var monthlyBillsAmount = familyDto.Budget.MonthlyBills.Sum(mb => mb.MonthlyAmount);
                var categoriesAmount = familyDto.Budget.BudgetCategories.Sum(c => c.Amount);

                familyDto.Budget.ActualRemaining = familyDto.Budget.PayThisMonth - monthlyBillsAmount - transactionsTotal;
                familyDto.Budget.ProjectRemaining = familyDto.Budget.PayThisMonth - monthlyBillsAmount - categoriesAmount;
            }

            return familyDto;
        }
    }
}
