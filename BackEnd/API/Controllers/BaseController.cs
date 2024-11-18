using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    // TODO: Move the user related repo calls to a repo class
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BaseController<T>(ILogger<T> logger) : ControllerBase
    {
        protected readonly ILogger<T> _logger = logger;
    }
}
