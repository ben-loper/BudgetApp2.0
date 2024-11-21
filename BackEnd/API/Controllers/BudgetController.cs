
using AutoMapper;
using BackEnd.DTOs.BudgetDtos;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    public class BudgetController : BaseController<BudgetController>
    {
        public BudgetController(ILogger<BudgetController> logger, IMapper mapper) : base(logger, mapper)
        {
        }

        [HttpPost]
        public async Task<ActionResult<BudgetDto>> CreateBudget(BudgetDto budget)
        {
            throw new NotImplementedException();
        }
    }
}
