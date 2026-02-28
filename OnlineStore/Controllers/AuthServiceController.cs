using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.AuthService;
using OnlineStore.Dtos;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthServiceController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthServiceController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<AuthDTO>> Register([FromBody] AuthRegisterDto register)
        {
            var authResult = await _authService.RegisterAsync(register);
            if (!authResult.IsSuccess)
                return NotFound(authResult.AuthData.Message);
            return Ok(authResult.AuthData);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<AuthDTO>> Login([FromBody] AuthLoginDto login)
        {
            var authResult = await _authService.LoginAsync(login);
            if (!authResult.IsSuccess)
                return NotFound(authResult.AuthData.Message);
            return Ok(authResult.AuthData);
        }
    }
}
