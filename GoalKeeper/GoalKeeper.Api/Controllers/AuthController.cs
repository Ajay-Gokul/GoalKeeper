using GoalKeeper.BusinessLogic.Interface;
using GoalKeeper.Model.DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;

namespace GoalKeeper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyCors")]

    public class AuthController : ControllerBase
    {
        private readonly IAuthProcessController<UserDTO> _authService;

        public AuthController(IAuthProcessController<UserDTO> authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var userData = await _authService.Login(request.Mail, request.Password);
            return Ok(new { userData });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _authService.Register(request);
            return Ok();
        }
    }
}
