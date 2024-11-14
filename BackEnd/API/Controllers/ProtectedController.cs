using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    public class ProtectedController : BaseController<ProtectedController>
    {
        public ProtectedController(ILogger<ProtectedController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) 
            : base(logger, userManager, signInManager)
        {
        }

        [HttpGet()]
        public ActionResult<string> Index()
        {
            return Ok("You are authorized!");
        }
    }
}
