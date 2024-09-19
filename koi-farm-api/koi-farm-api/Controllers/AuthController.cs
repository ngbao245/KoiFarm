using Microsoft.AspNetCore.Mvc;
using Repository.Model.Auth;
using Repository.Service;

namespace Repository.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignInModel signInModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var tokenResponse = _authService.SignIn(signInModel);
                return Ok(tokenResponse);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid credentials.");
            }
        }

        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] SignUpModel signUpModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var tokenResponse = _authService.SignUp(signUpModel);
                return Ok(tokenResponse);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Refresh token is required");

            try
            {
                var tokenResponse = _authService.RefreshToken(refreshToken);
                return Ok(tokenResponse);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }
        }
    }
}
