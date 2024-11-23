using AutoMapper;
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
    }
}
