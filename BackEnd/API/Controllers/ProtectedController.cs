using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProtectedController : ControllerBase
    {
        [HttpGet()]
        public ActionResult<string> Index()
        {
            return Ok("You are authorized!");
        }
    }
}
